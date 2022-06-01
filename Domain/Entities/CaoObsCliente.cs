using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoObsCliente : Entity
    {
        public uint CoObs { get; set; }
        public string Descricao { get; set; } = null!;
        public uint CoCliente { get; set; }
        public string? CoUsuario { get; set; }
        public DateTime? DtObs { get; set; }
    }
}
