using Application.Dtos;
using System.Collections.Generic;

namespace Application.IAppServices
{
    public partial interface ICaoUsuarioAppService : IAppService<CaoUsuarioDto>
    {
        List<UsuarioDto> GetUsuarios(IEnumerable<string> coUsuarios);
    }
}
