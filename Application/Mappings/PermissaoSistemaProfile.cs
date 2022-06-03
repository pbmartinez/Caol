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
                .ForMember(g => g.CaoUsuario, options => options.MapFrom(f => f.CaoUsuario))
                .ReverseMap();
        }
    }
}
