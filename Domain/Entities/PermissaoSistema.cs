using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class PermissaoSistema : Entity
    {
        public string CoUsuario { get; set; } = null!;
        public ulong CoTipoUsuario { get; set; }
        public ulong CoSistema { get; set; }
        public string InAtivo { get; set; } = null!;
        public string? CoUsuarioAtualizacao { get; set; }
        public DateTime DtAtualizacao { get; set; }
    }
}
