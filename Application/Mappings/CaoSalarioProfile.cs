using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class CaoSalarioProfile : Profile
    {
        public CaoSalarioProfile()
        {
            CreateMap<CaoSalario, CaoSalarioDto>()
                .ReverseMap();
        }
    }
}
