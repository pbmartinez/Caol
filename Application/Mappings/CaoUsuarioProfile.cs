using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class CaoUsuarioProfile : Profile
    {
        public CaoUsuarioProfile()
        {
            CreateMap<CaoUsuario, CaoUsuarioDto>()
                .ReverseMap();
        }
    }
}
