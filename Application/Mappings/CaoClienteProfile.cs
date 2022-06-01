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
                .ReverseMap();
        }
    }
}
