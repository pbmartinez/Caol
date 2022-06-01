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
                .ReverseMap();
        }
    }
}
