using System.Security.Claims;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApp;

public static class AppDataHelper
{
    public static async void SetupAppData(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration conf)
    {
        using var serviceScope = app
            .ApplicationServices
            .GetRequiredService<IServiceScopeFactory>()
            .CreateScope();

        using var context = serviceScope
            .ServiceProvider
            .GetService<AppDbContext>();

        if (context == null)
        {
            throw new ApplicationException("Problem in services. No DB context!");
        }

        // TODO - Check database state
        // can't connect - wrong address
        // can't connect - wrong user/pass
        // can connect - no database
        // can connect - database exists

        if (conf.GetValue<bool>("DataInitialization:DropDatabase")
            && !context.Database.IsInMemory())
        {
            context.Database.EnsureDeleted();
        }

        if (conf.GetValue<bool>("DataInitialization:MigrateDatabase") 
            && !context.Database.IsInMemory())
        {
            context.Database.Migrate();
        }

        if (conf.GetValue<bool>("DataInitialization:SeedIdentity"))
        {
            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();

            if (userManager == null || roleManager == null)
            {
                throw new NullReferenceException("userManager or roleManager cannot be null");
            }

            var roles = new[]
            {
                "admin",
                "user"
            };

            foreach (var roleName in roles)
            {
                var role = roleManager.FindByNameAsync(roleName).Result;

                if (role == null)
                {
                    var identityResult = roleManager.CreateAsync(new AppRole() {Name = roleName}).Result;

                    if (!identityResult.Succeeded)
                    {
                        throw new ApplicationException("Role creation failed");
                    }
                }
            }

            var users =
                new (string username, string firstname, string lastname, string roles, string password, string idCode)[]
                {
                    ("admin@itcollege.ee", "Suur", "Admin", "user,admin", "Kala.maja1", "38902180335"),
                    ("rasmus@itcollege.ee", "Rasmus", "Vare", "user", "Kala.maja1", "38002210335"),
                    ("user@itcollege.ee", "Tavaline", "Kasutaja", "user", "Kala.maja1", "39902180334"),
                    ("newuser@itcollege.ee", "Uus", "Kasutaja", "", "Kala.maja1", "38902184335"),
                };

            foreach (var userInfo in users)
            {
                var user = userManager.FindByEmailAsync(userInfo.username).Result;
                if (user == null)
                {
                    user = new AppUser
                    {
                        Email = userInfo.username,
                        FirstName = userInfo.firstname,
                        LastName = userInfo.lastname,
                        UserName = userInfo.username,
                        EmailConfirmed = true,
                    };
                    var identityResult = userManager.CreateAsync(user, userInfo.password).Result;
                    identityResult = userManager.AddClaimAsync(user, new Claim("aspnet.firstname", user.FirstName))
                        .Result;
                    identityResult = userManager.AddClaimAsync(user, new Claim("aspnet.lastname", user.LastName))
                        .Result;

                    if (!identityResult.Succeeded)
                    {
                        throw new AggregateException("Cannot create user");
                    }

                    var person = new Person
                    {
                        AppUserId = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        IdCode = userInfo.idCode,
                        IsMain = true,
                    };

                    context.Persons.Add(person);
                    await context.SaveChangesAsync();
                }

                if (!string.IsNullOrWhiteSpace(userInfo.roles))
                {
                    var identityResultRoles = userManager.AddToRolesAsync(user,
                            userInfo.roles.Split(",").Select(r => r.Trim()))
                        .Result;
                }
            }
        }

        if (conf.GetValue<bool>("DataInitialization:SeedData") 
            && !context.Database.IsInMemory()
            )
        {
            // Add associations
            var associations = new (string name, string registrationNo)[]
            {
                ("Test association", "123456789"),
                ("Association test", "987654321")
            };
            var associationIds = new List<Guid>();

            foreach (var associationInfo in associations)
            {
                var association = context.Associations.FirstOrDefaultAsync(m => m.Name == associationInfo.name).Result;
                if (association == null)
                {
                    association = new Association
                    {
                        Name = associationInfo.name,
                        RegistrationNo = associationInfo.registrationNo,
                        FoundedOn = DateOnly.FromDateTime(DateTime.UtcNow)
                    };
                    context.Associations.Add(association);
                    await context.SaveChangesAsync();
                    associationIds.Add(association.Id);
                }
            }

            // Add global measuring units
            var measuringUnits = new (string name, string description, string symbol)[]
            {
                ("Cubic meters", "cubic meters", "m3"),
                ("Kilowatt hour", "kilowatt hour", "kWh")
            };

            foreach (var unitInfo in measuringUnits)
            {
                var measuringUnit = context.MeasuringUnits.FirstOrDefaultAsync(m => m.Name == unitInfo.name).Result;

                if (measuringUnit == null)
                {
                    measuringUnit = new MeasuringUnit
                    {
                        Name = unitInfo.name,
                        Description = unitInfo.description,
                        Symbol = unitInfo.symbol
                    };

                    context.MeasuringUnits.Add(measuringUnit);
                    await context.SaveChangesAsync();
                }
            }

            // Add global meter types

            var meterTypes = new (string name, string description, int daysbtwchecks, string measuringUnit)[]
            {
                ("Cold water", "Meter for cold water consumption", 365, "Cubic meters"),
                ("Hot water", "Meter for hot water consumption", 365, "Cubic meters"),
                ("Electricity", "Meter for electricity consumption", 365, "Kilowatt hour"),
            };

            foreach (var meterTypeInfo in meterTypes)
            {
                var meterType = context.MeterTypes.FirstOrDefaultAsync(m => m.Name == meterTypeInfo.name).Result;

                if (meterType == null)
                {
                    var measutringunit =
                        context.MeasuringUnits.FirstOrDefaultAsync(m => m.Name == meterTypeInfo.measuringUnit).Result;

                    meterType = new MeterType()
                    {
                        Name = meterTypeInfo.name,
                        Description = meterTypeInfo.description,
                        MeasuringUnitId = measutringunit!.Id,
                        DaysBtwChecks = meterTypeInfo.daysbtwchecks
                    };
                    context.MeterTypes.Add(meterType);
                    await context.SaveChangesAsync();
                }
            }

            // Add global contact types
            var contactTypes = new (string name, string description)[]
            {
                ("Email", "Email"),
                ("Address", "address"),
                ("Phone", "phone"),
            };

            foreach (var contactTypeInfo in contactTypes)
            {
                var contactType = context.ContactTypes.FirstOrDefaultAsync(c => c.Name == contactTypeInfo.name).Result;

                if (contactType == null)
                {
                    contactType = new ContactType
                    {
                        Name = contactTypeInfo.name,
                        Description = contactTypeInfo.description
                    };
                    context.ContactTypes.Add(contactType);
                    await context.SaveChangesAsync();
                }
            }

            // foreach (var unitInfo in measuringUnits)
            // {
            //     var measuringUnit = context.MeasuringUnits.FirstOrDefaultAsync(m => m.Name == unitInfo.name).Result;
            //
            //     if (measuringUnit == null)
            //     {
            //         measuringUnit = new MeasuringUnit
            //         {
            //             Name = unitInfo.name,
            //             Description = unitInfo.description,
            //             Symbol = unitInfo.symbol
            //         };
            //
            //         context.MeasuringUnits.Add(measuringUnit);
            //         await context.SaveChangesAsync();
            //     }
            // }

            // Add member types to the associations
            var memberTypes = new (string name, string description, bool isMemberOfBoard, bool isAdministratror)[]
            {
                ("admin", "administrator", true, true),
                ("member of board", "member of board", true, false),
                ("regular member", "regular member", false, false),
            };
            foreach (var memberTypeInfo in memberTypes)
            {
                foreach (var associationId in associationIds)
                {
                    var memberType = context.MemberTypes
                        .FirstOrDefaultAsync(a => a.Id == associationId
                                                  && a.Name == memberTypeInfo.name).Result;

                    if (memberType == null)
                    {
                        memberType = new MemberType
                        {
                            Name = memberTypeInfo.name,
                            Description = memberTypeInfo.description,
                            IsAdministrator = memberTypeInfo.isAdministratror,
                            IsMemberOfBoard = memberTypeInfo.isMemberOfBoard,
                            IsManager = false,
                            IsAccountant = false,
                            AssociationId = associationId
                        };
                        context.MemberTypes.Add(memberType);
                        await context.SaveChangesAsync();
                    }
                }
            }

            // Add members to the asscociations

            var members = new (string idCode, string association, string memberType)[]
            {
                ("38902180335", "Test association", "admin"),
                ("38002210335", "Test association", "regular member"),
                ("38002210335", "Association test", "admin"),
                // ("38902184335", "Test association", "member of board"),
                ("39902180334", "Association test", "regular member"),
                ("39902180334", "Test association", "regular member"),
            };

            foreach (var memberInfo in members)
            {
                var person = await context.Persons.FirstOrDefaultAsync(p => p.IdCode == memberInfo.idCode);
                var association = await context.Associations.FirstOrDefaultAsync(a => a.Name == memberInfo.association);

                var membertype = await context.MemberTypes.FirstOrDefaultAsync(m =>
                    m.AssociationId == association!.Id && m.Name == memberInfo.memberType);

                var member = await context.Members.FirstOrDefaultAsync(m =>
                    m.AssociationId == association!.Id && m.PersonId == person!.Id);

                if (member == null)
                {
                    member = new Member
                    {
                        PersonId = person!.Id,
                        AssociationId = association!.Id,
                        MemberTypeId = membertype!.Id,
                        ViewAsRegularUser = memberInfo.memberType == "admin" ? false : true,
                        From = DateOnly.FromDateTime(DateTime.UtcNow)
                    };
                }

                context.Members.Add(member);
                await context.SaveChangesAsync();
            }

            // Add buildings, apartments and meters to the associations

            var buildings = new (string name, int noOfApts, decimal commonSqM, decimal ApartmentSqM, decimal totalSqM)[]
            {
                ("Sample building", 4, 250, 400, 650)
            };

            var apartments = new (string aptNo, decimal totalSqM, int noOfRooms)[]
            {
                ("1", 100, 4),
                ("2", 100, 3),
                ("3", 100, 2),
                ("4", 100, 5),
            };

            foreach (var buildingInfo in buildings)
            {
                foreach (var associationId in associationIds)
                {
                    var building = context.Buildings.FirstOrDefaultAsync(b =>
                        b.AssociationId == associationId && b.Name == buildingInfo.name).Result;

                    if (building == null)
                    {
                        building = new Building
                        {
                            Name = buildingInfo.name,
                            AssociationId = associationId,
                            NoOfApartments = buildingInfo.noOfApts,
                            CommonSqM = buildingInfo.commonSqM,
                            ApartmentSqM = buildingInfo.ApartmentSqM,
                            BusinessSqM = 0,
                            TotalSqM = buildingInfo.totalSqM
                        };
                        context.Buildings.Add(building);
                        await context.SaveChangesAsync();

                        foreach (var apartmentInfo in apartments)
                        {
                            var apartment = new Apartment
                            {
                                AptNumber = apartmentInfo.aptNo,
                                TotalSqMtr = apartmentInfo.totalSqM,
                                NoOfRooms = apartmentInfo.noOfRooms,
                                BuildingId = building.Id
                            };
                            context.Apartments.Add(apartment);
                            await context.SaveChangesAsync();

                            var meterType = context.MeterTypes.FirstOrDefaultAsync(m => m.Name == "Cold water")
                                .Result;
                            var meter = new Meter
                            {
                                ApartmentId = apartment.Id,
                                BuildingId = building.Id,
                                MeterTypeId = meterType!.Id,
                                MeterNumber = "ADRET454321",
                                InstalledOn = DateOnly.FromDateTime(DateTime.UtcNow)
                            };
                            context.Meters.Add(meter);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }

            // Add persons to the apartments

            foreach (var associationId in associationIds)
            {
                var associationBuilding = context.Buildings
                    .FirstOrDefaultAsync(b => b.AssociationId == associationId).Result;
                var associationApartments = context.Apartments.Where(a => a.BuildingId == associationBuilding!.Id)
                    .ToListAsync().Result;
                var associationMembers =
                    context.Members.Where(m => m.AssociationId == associationId).ToListAsync().Result;

                var memberIndex = 0;
                foreach (var associationApartment in associationApartments)
                {
                    // foreach (var associationMember in associationMembers)
                    // {
                    var associationMember = associationMembers[memberIndex];
                    var personId = associationMember.PersonId;

                    var apartmentPerson = context.ApartmentPersons.FirstOrDefaultAsync(a =>
                            a.PersonId == personId && a.ApartmentId == associationApartment.Id)
                        .Result;
                    // Console.WriteLine(apartmentPerson.PersonId);

                    if (apartmentPerson == null)
                    {
                        apartmentPerson = new ApartmentPerson
                        {
                            ApartmentId = associationApartment.Id,
                            PersonId = personId,
                            IsOwner = true,
                            From = DateOnly.FromDateTime(DateTime.UtcNow)
                        };

                        context.ApartmentPersons.Add(apartmentPerson);
                        await context.SaveChangesAsync();
                    }

                    if (memberIndex < associationMembers.Count - 1)
                    {
                        memberIndex++;
                    }
                    // }
                }

                // var personsApartments =
                //     associationApartments.Zip(personIds, (a, p) => new {a = associationApartments, p = personIds});
                // foreach (var apartment in associationApartments)
                // {
                //     
                // }
                //
                // var apartmentPerson = context.ApartmentPersons.FirstOrDefaultAsync().Result;
            }


            // var associations = new (string username, string firstname, string lastname, string roles, string password, string idCode)[]
            // {
            //     ("admin@itcollege.ee", "Suur", "Admin", "user,admin", "Kala.maja1", "38902180335"),
            //     ("rasmus@itcollege.ee", "Rasmus", "Vare", "user,admin", "Kala.maja1", "38002210335"),
            /*
             * var f = new FooBar
             * {
             *      Name =
             *      {
             *          ["en-GB"] = "English"
             *          ["et-EE"] = "Estonian"
             *          ["ua-UA"] = "Ukrainian"
             *      }
             * };
             * context.FooBars.Add(f);
             * context.SaveChanges();
             */
        }
    }
}