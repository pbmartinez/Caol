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
                .ReverseMap();
        }
    }
}
