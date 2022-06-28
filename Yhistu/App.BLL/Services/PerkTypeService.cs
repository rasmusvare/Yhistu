using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class PerkTypeService : BaseEntityService<App.BLL.DTO.PerkType, App.DAL.DTO.PerkType, IPerkTypeRepository>,
    IPerkTypeService
{
    public PerkTypeService(IPerkTypeRepository repository, IMapper<PerkType, DAL.DTO.PerkType> mapper) : base(
        repository, mapper)
    {
    }
}