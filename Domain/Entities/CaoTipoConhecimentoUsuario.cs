using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoTipoConhecimentoUsuario : Entity
    {
        public uint CoConhecimento { get; set; }
        public string? DsConhecimento { get; set; }
        public uint CoSistema { get; set; }
    }
}
