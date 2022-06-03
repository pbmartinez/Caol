using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class CaoClienteProfile : Profile
    {
        public CaoClienteProfile()
        {
            CreateMap<CaoCliente, CaoClienteDto>()
                .ForMember(g => g.CaoFaturas, options => options.MapFrom(f => f.CaoFaturas))
                .ReverseMap();
        }
    }
}
