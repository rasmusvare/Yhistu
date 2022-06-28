using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class PersonService : BaseEntityService<App.BLL.DTO.Person, App.DAL.DTO.Person, IPersonRepository>,
    IPersonService
{
    public PersonService(IPersonRepository repository, IMapper<Person, DAL.DTO.Person> mapper) : base(repository,
        mapper)
    {
    }

    public async Task<Guid> GetMainPersonId(Guid userId, bool noTracking = true)
    {
        return await Repository.GetMainPersonId(userId);
    }

    public async Task<IEnumerable<Person>> GetAllAsync(Guid personId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(personId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Person>> GetAllAssociationAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetAllAssociationAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);
    }
    
    public async Task<IEnumerable<Person>> GetBoardMembersAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetBoardMembersAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<Person?> FirstOrDefaultAsync(Person person)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(Mapper.Map(person)!));
    }

    public async Task<Person?> FindByEmail(string email)
    {
        return Mapper.Map(await Repository.FindByEmail(email));
    }

    public override Person Add(Person entity)
    {
        
        return base.Add(entity);
    }
}
