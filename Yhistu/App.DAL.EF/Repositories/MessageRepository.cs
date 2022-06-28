using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class MessageRepository : BaseEntityRepository<App.DAL.DTO.Message, App.Domain.Message, AppDbContext>,
    IMessageRepository
{
    public MessageRepository(AppDbContext dbContext, IMapper<Message, Domain.Message> mapper) : base(dbContext, mapper)
    {
    }
}