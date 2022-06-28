using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class MessageMapper : BaseMapper<App.BLL.DTO.Message, App.DAL.DTO.Message>
{
    public MessageMapper(IMapper mapper) : base(mapper)
    {
    }
}