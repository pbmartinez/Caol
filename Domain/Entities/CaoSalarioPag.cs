using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoSalarioPag : Entity
    {
        public int IdPagamento { get; set; }
        public string CoUsuario { get; set; } = null!;
        public DateOnly DtEfetuado { get; set; }
        public string Status { get; set; } = null!;
        public string Observacao { get; set; }

        public virtual CaoUsuario CoUsuarioNavigation { get; set; } = null!;
    }
}
