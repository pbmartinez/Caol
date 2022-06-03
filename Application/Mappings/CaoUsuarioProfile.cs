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
                .ForMember(g => g.PermissaoSistema, options => options.MapFrom(f => f.PermissaoSistema))
                .ForMember(g => g.CaoOrdenesServicios, options => options.MapFrom(f => f.CaoOrdenesServicios))
                .ForMember(g => g.CaoSalario, options => options.MapFrom(f => f.CaoSalario))
                .ReverseMap();
        }
    }
}
