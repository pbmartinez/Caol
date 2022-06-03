using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class CaoOProfile : Profile
    {
        public CaoOProfile()
        {
            CreateMap<CaoO, CaoODto>()
                .ForMember(g => g.CaoFaturas, options => options.MapFrom(f => f.CaoFaturas))
                .ForMember(g => g.CaoUsuario, options => options.MapFrom(f => f.CaoUsuario))
                .ReverseMap();
        }
    }
}
