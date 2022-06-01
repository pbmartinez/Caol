using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoSistemaOb : Entity
    {
        public uint CoObs { get; set; }
        public string? Descricao { get; set; }
        public long? CoSistema { get; set; }
        public string? CoUsuario { get; set; }
        public DateTime? DtObs { get; set; }
    }
}
