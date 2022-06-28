using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// using ApartmentPerson = App.DAL.DTO.ApartmentPerson;
// using BankAccount = App.DAL.DTO.BankAccount;
// using CalculationRules = App.DAL.DTO.CalculationRules;
// using Contact = App.DAL.DTO.Contact;
// using ContactType = App.DAL.DTO.ContactType;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public DbSet<Apartment> Apartments { get; set; } = default!;
    public DbSet<ApartmentPerson> ApartmentPersons { get; set; } = default!;
    public DbSet<Association> Associations { get; set; } = default!;
    public DbSet<AssociationService> AssociationServices { get; set; } = default!;
    public DbSet<BankAccount> BankAccounts { get; set; } = default!;
    public DbSet<Building> Buildings { get; set; } = default!;
    public DbSet<CalculationRules> CalculationRules { get; set; } = default!;
    public DbSet<Contact> Contacts { get; set; } = default!;
    public DbSet<ContactType> ContactTypes { get; set; } = default!;
    public DbSet<Invoice> Invoices { get; set; } = default!;
    public DbSet<InvoiceRow> InvoiceRows { get; set; } = default!;
    public DbSet<InvoiceType> InvoiceTypes { get; set; } = default!;
    public DbSet<MeasuringUnit> MeasuringUnits { get; set; } = default!;
    public DbSet<Member> Members { get; set; } = default!;
    public DbSet<MemberType> MemberTypes { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<Meter> Meters { get; set; } = default!;
    public DbSet<MeterReading> MeterReadings { get; set; } = default!;
    public DbSet<MeterType> MeterTypes { get; set; } = default!;
    public DbSet<Perk> Perks { get; set; } = default!;
    public DbSet<PerkType> PerkTypes { get; set; } = default!;
    public DbSet<Person> Persons { get; set; } = default!;
    public DbSet<RelationshipType> RelationshipTypes { get; set; } = default!;
    public DbSet<Service> Services { get; set; } = default!;
    public DbSet<ServiceProvider> ServiceProviders { get; set; } = default!;
    public DbSet<ServiceType> ServiceTypes { get; set; } = default!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    // public DbSet<MeetingOption> MeetingOptions { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Remove cascade delete
        foreach (var relationship in builder.Model
                     .GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    public override int SaveChanges()
    {
        FixEntities(this);
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        FixEntities(this);
        return base.SaveChangesAsync(cancellationToken);
    }

    private void FixEntities(AppDbContext context)
    {
        var dateProperties = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime))
            .Select(z => new
            {
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            });

        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(x => x.Entity);


        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as DateTime?;
                if (originalValue == null)
                    continue;

                prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
            }
        }
    }
}