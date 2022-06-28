using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class MemberTypeService:BaseEntityService<App.BLL.DTO.MemberType, App.DAL.DTO.MemberType, IMemberTypeRepository>,
    IMemberTypeService
{
    public MemberTypeService(IMemberTypeRepository repository, IMapper<MemberType, DAL.DTO.MemberType> mapper) : base(repository, mapper)
    {
    }

    public async Task<Guid?> GetAdminId()
    {
        //TODO: Doesn't really work
        return await Repository.GetAdminId();
    }

    public Task<bool> IsUsed(Guid id)
    {
        return Repository.IsUsed(id);
    }

    public MemberType CreateDefaultAdminMemberType(Guid associationId)
    {
        var defaultAdmin = new App.DAL.DTO.MemberType
        {
            Name = "Administrator",
            Description = "Administrator",
            IsAdministrator = true,
            IsAccountant = false,
            IsManager = false,
            IsRegularMember = true,
            IsMemberOfBoard = true,
            AssociationId = associationId
        };

        return Mapper.Map(defaultAdmin)!;
    }

    public async Task<IEnumerable<MemberType>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}