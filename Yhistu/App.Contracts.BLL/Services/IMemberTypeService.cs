using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMemberTypeService : IEntityService<App.BLL.DTO.MemberType>, IMemberTypeRepositoryCustom<App.BLL.DTO.MemberType>
{
    App.BLL.DTO.MemberType CreateDefaultAdminMemberType(Guid associationId);
    Task<IEnumerable<MemberType>> GetAllAsync(Guid associationId, bool noTracking = true);

}