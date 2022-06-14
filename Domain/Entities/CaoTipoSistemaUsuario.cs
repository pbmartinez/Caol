using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoTipoSistemaUsuario : Entity
    {
        public uint CoSistema { get; set; }
        public string DsSistema { get; set; }
    }
}
