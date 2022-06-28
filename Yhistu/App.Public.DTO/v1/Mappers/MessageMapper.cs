using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class MessageMapper : BaseMapper<Message, App.BLL.DTO.Message>
{
    public MessageMapper(IMapper mapper) : base(mapper)
    {
    }
}