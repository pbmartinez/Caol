using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class CaoFaturaProfile : Profile
    {
        public CaoFaturaProfile()
        {
            CreateMap<CaoFatura, CaoFaturaDto>()
                .ForMember(g => g.CaoCliente, options => options.MapFrom(f => f.CaoCliente))
                .ForMember(g => g.CaoOrdenServicio, options => options.MapFrom(f => f.CaoOrdenServicio))
                .ForMember(g => g.CaoSistema, options => options.MapFrom(f => f.CaoSistema))
                .ReverseMap();
        }
    }
}
