using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class TipoUsuario : Entity
    {
        public ulong CoTipoUsuario { get; set; }
        public string DsTipoUsuario { get; set; } = null!;
        public ulong CoSistema { get; set; }
    }
}
