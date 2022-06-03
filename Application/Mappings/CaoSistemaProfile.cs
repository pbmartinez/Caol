using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class CaoSistemaProfile : Profile
    {
        public CaoSistemaProfile()
        {
            CreateMap<CaoSistema, CaoSistemaDto>()
                //Enable mapping when set navigation property
                //.ForMember(g => g.CaoFatura, options => options.MapFrom(f => f.CaoFatura))
                .ReverseMap();
        }
    }
}
