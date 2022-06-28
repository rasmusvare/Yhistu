using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.BLL.DTO;
using App.Contracts.BLL;
using App.DAL.EF;
using App.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace Testing.WebApp.Helpers;

public class TestDataHelper
{
    private readonly IAppBLL _bll;
    private readonly AppDbContext _context;

    public TestDataHelper(AppDbContext context, IAppBLL bll)
    {
        _bll = bll;
        _context = context;
    }

    public async Task SeedData()
    {
        var users =
            new (string username, string firstname, string lastname, string roles, string password, string idCode)[]
            {
                ("admin@itcollege.ee", "Suur", "Admin", "user,admin", "Kala.maja1", "38902180335"),
                ("rasmus@itcollege.ee", "Rasmus", "Vare", "user", "Kala.maja1", "38002210335"),
            };

        foreach (var userInfo in users)
        {
            var user = new AppUser
            {
                Email = userInfo.username,
                FirstName = userInfo.firstname,
                LastName = userInfo.lastname,
                UserName = userInfo.username,
                EmailConfirmed = true,
                PasswordHash = "PasswordHash123",
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var appUserId = user.Id;

            var person = new Person
            {
                AppUserId = appUserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IdCode = userInfo.idCode,
                IsMain = true,
            };

            _bll.Persons.Add(person);
            await _bll.SaveChangesAsync();
        }


        // Add associations
        var associations = new (string name, string registrationNo)[]
        {
            ("Test association", "123456789"),
            ("Association test", "987654321")
        };

        var associationIds = new List<Guid>();
        foreach (var associationInfo in associations)
        {
            var association = new Association
            {
                Name = associationInfo.name,
                RegistrationNo = associationInfo.registrationNo,
                FoundedOn = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            _bll.Associations.Add(association);
            await _bll.SaveChangesAsync();
            association.Id = _bll.Associations.GetIdFromDb(association);
            associationIds.Add(association.Id);
        }

        // Add global measuring unit

        var measuringUnit = new MeasuringUnit
        {
            Name = "Cubic meters",
            Description = "cubic meters",
            Symbol = "m3"
        };

        _bll.MeasuringUnits.Add(measuringUnit);
        await _bll.SaveChangesAsync();
        measuringUnit.Id = _bll.MeasuringUnits.GetIdFromDb(measuringUnit);


        // Add global meter types

        var meterTypes = new (string name, string description, int daysbtwchecks, string measuringUnit)[]
        {
            ("Cold water", "Meter for cold water consumption", 365, "Cubic meters"),
        };
        foreach (var meterTypeInfo in meterTypes)
        {
            var meterType = new MeterType()
            {
                Name = meterTypeInfo.name,
                Description = meterTypeInfo.description,
                MeasuringUnitId = measuringUnit.Id,
                DaysBtwChecks = meterTypeInfo.daysbtwchecks
            };
            _bll.MeterTypes.Add(meterType);
            await _bll.SaveChangesAsync();
        }

        // Add global contact types
        var contactTypes = new (string name, string description)[]
        {
            ("Email ", "Email"),
            ("Address", "address"),
            ("Phone", "phone"),
        };
        foreach (var contactTypeInfo in contactTypes)
        {
            var contactType = new ContactType
            {
                Name = contactTypeInfo.name,
                Description = contactTypeInfo.description
            };
            _bll.ContactTypes.Add(contactType);
            await _bll.SaveChangesAsync();
        }

        // Add member types to the associations
        var memberTypes = new (string name, string description, bool isMemberOfBoard, bool isAdministratror)[]
        {
            ("admin", "administrator", true, true),
            ("regular member", "regular member", false, false),
        };
        foreach (var memberTypeInfo in memberTypes)
        {
            foreach (var associationId in associationIds)
            {
                var memberType = new MemberType
                {
                    Name = memberTypeInfo.name,
                    Description = memberTypeInfo.description,
                    IsAdministrator = memberTypeInfo.isAdministratror,
                    IsMemberOfBoard = memberTypeInfo.isMemberOfBoard,
                    IsManager = false,
                    IsAccountant = false,
                    AssociationId = associationId
                };
                _bll.MemberTypes.Add(memberType);
                await _bll.SaveChangesAsync();
            }
        }


        // Add members to the asscociations

        var members = new (string idCode, string association, string memberType)[]
        {
            ("38902180335", "Test association", "admin"),
            ("38002210335", "Test association", "regular member"),
            ("38902180335", "Association test", "admin"),
        };
        foreach (var memberInfo in members)
        {
            var personsDb = await _bll.Persons.GetAllAsync();
            var associationsDb = await _bll.Associations.GetAllAsync();
            var memberTypesDb = await _bll.MemberTypes.GetAllAsync();

            var person = personsDb.First(p => p.IdCode == memberInfo.idCode);

            var association = associationsDb.First(a => a.Name == memberInfo.association);

            var membertype =
                memberTypesDb.First(mt => mt.Name == memberInfo.memberType
                                          && mt.AssociationId == association.Id
                );


            var member = new Member
            {
                PersonId = person.Id,
                AssociationId = association.Id,
                MemberTypeId = membertype.Id,
                ViewAsRegularUser = memberInfo.memberType != "admin",
                From = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            _bll.Members.Add(member);
            await _bll.SaveChangesAsync();
            member.Id = _bll.Members.GetIdFromDb(member);

            // if(member.MemberType.)
        }


        // Add buildings, apartments and meters to the associations

        var buildings = new (string name, int noOfApts, decimal commonSqM, decimal ApartmentSqM, decimal totalSqM)[]
        {
            ("Sample building", 2, 250, 200, 450)
        };

        var apartments = new (string aptNo, decimal totalSqM, int noOfRooms)[]
        {
            ("1", 100, 4),
            ("2", 100, 3),
        };
        foreach (var buildingInfo in buildings)
        {
            foreach (var associationId in associationIds)
            {
                var building = new Building
                {
                    Name = buildingInfo.name,
                    AssociationId = associationId,
                    NoOfApartments = buildingInfo.noOfApts,
                    CommonSqM = buildingInfo.commonSqM,
                    ApartmentSqM = buildingInfo.ApartmentSqM,
                    BusinessSqM = 0,
                    TotalSqM = buildingInfo.totalSqM
                };
                _bll.Buildings.Add(building);
                await _bll.SaveChangesAsync();
                building.Id = _bll.Buildings.GetIdFromDb(building);

                foreach (var apartmentInfo in apartments)
                {
                    var apartment = new Apartment
                    {
                        AptNumber = apartmentInfo.aptNo,
                        TotalSqMtr = apartmentInfo.totalSqM,
                        NoOfRooms = apartmentInfo.noOfRooms,
                        BuildingId = building.Id
                    };
                    _bll.Apartments.Add(apartment);
                    await _bll.SaveChangesAsync();
                    apartment.Id = _bll.Apartments.GetIdFromDb(apartment);

                    var meterTypesDb = await _bll.MeterTypes.GetAllAsync();

                    var meterType = meterTypesDb.First(mt => mt.Name == "Cold water");

                    var meter = new Meter
                    {
                        ApartmentId = apartment.Id,
                        BuildingId = building.Id,
                        MeterTypeId = meterType!.Id,
                        MeterNumber = "ADRET454321",
                        InstalledOn = DateOnly.FromDateTime(DateTime.UtcNow)
                    };
                    _bll.Meters.Add(meter);
                    await _bll.SaveChangesAsync();
                }
            }
        }

        // Add persons to the apartments

        foreach (var ascId in associationIds)
        {
            var associationBuilding = await _bll.Buildings.GetAllAsync(ascId);
            var buildingApartments =
                await _bll.Apartments.GetAllInBuildingAsync(associationBuilding.First().Id);
            var associationMembers = (await _bll.Members.GetAllAsync(ascId)).ToList();

            // var associationBuilding = context.Buildings
            //     .FirstOrDefaultAsync(b => b.AssociationId == associationId).Result;
            // var associationApartments = context.Apartments.Where(a => a.BuildingId == associationBuilding!.Id)
            //     .ToListAsync().Result;
            // var associationMembers =
            //     context.Members.Where(m => m.AssociationId == associationId).ToListAsync().Result;

            var memberIndex = 0;
            foreach (var apartment in buildingApartments)
            {
                var associationMember = associationMembers[memberIndex];
                var personId = associationMember.PersonId;

                var apartmentPerson = new ApartmentPerson
                {
                    ApartmentId = apartment.Id,
                    PersonId = personId,
                    IsOwner = true,
                    From = DateOnly.FromDateTime(DateTime.UtcNow)
                };

                _bll.ApartmentPersons.Add(apartmentPerson);
                await _bll.SaveChangesAsync();


                if (memberIndex < associationMembers.Count - 1)
                {
                    memberIndex++;
                }
            }
        }
    }

    public async Task<Guid> GetAdminPersonId()
    {
        var member = _context.Members
            .Include(m => m.MemberType)
            .Include(m => m.Person)
            .First(m => m.MemberType!.Name == "admin");

        Console.WriteLine(member.Person.FirstName);

        return member.Person.Id;
    }

    public void ClearEntityCache()
    {
        _context.ChangeTracker.Clear();
    }
}