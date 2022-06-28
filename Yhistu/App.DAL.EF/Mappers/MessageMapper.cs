using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MessageMapper : BaseMapper<App.DAL.DTO.Message, App.Domain.Message>
{
    public MessageMapper(IMapper mapper) : base(mapper)
    {
    }
}