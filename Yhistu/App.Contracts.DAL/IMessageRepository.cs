using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IMessageRepository: IEntityRepository<Message>
{
    // Custom methods here
}