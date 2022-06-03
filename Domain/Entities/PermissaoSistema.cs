using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class PermissaoSistema : Entity
    {
        public string CoUsuario { get; set; } = null!;
        public ulong CoTipoUsuario { get; set; }
        public ulong CoSistema { get; set; }
        public string InAtivo { get; set; } = null!;
        public string CoUsuarioAtualizacao { get; set; }
        public DateTime DtAtualizacao { get; set; }

        [ForeignKey("CoUsuario")]
        public virtual CaoUsuario CaoUsuario { get; set;}
    }
}
