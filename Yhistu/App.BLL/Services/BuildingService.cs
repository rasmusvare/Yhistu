using System.Net;
using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class BuildingService : BaseEntityService<App.BLL.DTO.Building, App.DAL.DTO.Building, IBuildingRepository>,
    IBuildingService
{
    public BuildingService(IBuildingRepository repository, IMapper<Building, DAL.DTO.Building> mapper) : base(
        repository, mapper)
    {
    }

    public async Task<IEnumerable<Building>> GetAllAsync(Guid associationId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(associationId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    // public async Task CalculateAreas(Guid buildingId)
    public async Task CalculateAreas(Building building)
    {
        var buildingDb = await Repository.GetWithApartmentsAsync(building.Id);

        if (buildingDb?.Apartments == null)
        {
            return;
            // Mapper.Map(building)!;
        }

        decimal apartmentSqMtrs = 0;
        decimal businessSqMtrs = 0;
        building.NoOfApartments = buildingDb.Apartments.Count;

        foreach (var apartment in buildingDb.Apartments)
        {
            if (!apartment.IsBusiness)
            {
                apartmentSqMtrs += apartment.TotalSqMtr;
            }
            else
            {
                businessSqMtrs += apartment.TotalSqMtr;
            }
        }

        building.ApartmentSqM = apartmentSqMtrs;
        building.BusinessSqM = businessSqMtrs;
        building.TotalSqM = apartmentSqMtrs + businessSqMtrs + building.CommonSqM;

        Update(building);

        // return Mapper.Map(building)!;
    }
}