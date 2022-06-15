using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoPermissao : Entity
    {
        public string CoUsuario { get; set; } = null!;
        public string PermissaoIntervalo { get; set; } = null!;
        public string PermissaoHoraExtra { get; set; } = null!;

        public virtual CaoUsuario CoUsuarioNavigation { get; set; } = null!;
    }
}
