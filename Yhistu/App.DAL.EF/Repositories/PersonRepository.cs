using App.Contracts.DAL;
using App.Domain;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Person = App.DAL.DTO.Person;

namespace App.DAL.EF.Repositories;

public class PersonRepository : BaseEntityRepository<App.DAL.DTO.Person, App.Domain.Person, AppDbContext>,
    IPersonRepository
{
    public PersonRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.Person, App.Domain.Person> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<Guid> GetMainPersonId(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var person = await query.FirstOrDefaultAsync(a => a.AppUserId == userId && a.IsMain);

        Console.WriteLine(person == null);
        return person!.Id;
    }

    public async Task<IEnumerable<Person>> GetAllAsync(Guid appUserId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Where(p => p.AppUserId == appUserId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Person>> GetAllAssociationAsync(Guid associationId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(p => p.Members)
            .Include(p => p.Contacts)!
            .ThenInclude(c => c.ContactType)
            .SelectMany(p => p.Members!
                .Where(m => m.AssociationId == associationId))
            .Select(m => m.Person)!;
       
        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Person>> GetBoardMembersAsync(Guid associationId, bool noTracking)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(p => p.Members!)
            .ThenInclude(m => m.MemberType)
            .SelectMany(p => p.Members!
                .Where(m => m.AssociationId == associationId && (m.MemberType!.IsAdministrator == true ||
                                                                 m.MemberType.IsMemberOfBoard == true ||
                                                                 m.MemberType.IsManager == true)))
            .Select(m => m.Person)!;

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<Person?> FirstOrDefaultAsync(Person person)
    {
        var query = CreateQuery();

        var personDb = await query.FirstOrDefaultAsync(p => p.Email == person.Email && p.IdCode == person.IdCode);

        return Mapper.Map(personDb);
    }

    public async Task<Person?> FindByEmail(string email)
    {
        var query = CreateQuery();

        var personDb =await query.Include(p => p.Contacts!)
            .ThenInclude(c=>c.Person!)
            .Include(p => p.Contacts!)
            .ThenInclude(c=>c.ContactType!)
            .Select(p =>  p.Contacts!
                .FirstOrDefault(c => c.ContactType!.Name == "Email" && c.Value == email)!)
            .Select(c=>c.Person).FirstAsync();

        return Mapper.Map(personDb);
    }
}