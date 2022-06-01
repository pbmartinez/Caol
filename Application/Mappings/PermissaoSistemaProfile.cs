using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class PermissaoSistemaProfile : Profile
    {
        public PermissaoSistemaProfile()
        {
            CreateMap<PermissaoSistema, PermissaoSistemaDto>()
                .ReverseMap();
        }
    }
}
