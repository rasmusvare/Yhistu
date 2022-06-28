using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class PerkService : BaseEntityService<App.BLL.DTO.Perk, App.DAL.DTO.Perk, IPerkRepository>,
    IPerkService
{
    public PerkService(IPerkRepository repository, IMapper<Perk, DAL.DTO.Perk> mapper) : base(repository, mapper)
    {
    }
}