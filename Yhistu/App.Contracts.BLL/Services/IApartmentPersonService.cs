using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IApartmentPersonService: IEntityService<App.BLL.DTO.ApartmentPerson>, IApartmentPersonRepositoryCustom<App.BLL.DTO.ApartmentPerson>
{
    // Task<IEnumerable<App.BLL.DTO.Apartment>> GetAllAsync(Guid userId, bool noTracking = true);

}