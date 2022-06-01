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
                .ReverseMap();
        }
    }
}
