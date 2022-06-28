using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class MessageService : BaseEntityService<App.BLL.DTO.Message, App.DAL.DTO.Message, IMessageRepository>,
    IMessageService
{
    public MessageService(IMessageRepository repository, IMapper<Message, DAL.DTO.Message> mapper) : base(repository,
        mapper)
    {
    }
}