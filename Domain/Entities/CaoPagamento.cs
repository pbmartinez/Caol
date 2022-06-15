using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoPagamento : Entity
    {
        public ulong CoPagamento { get; set; }
        public string CoUsuario { get; set; } = null!;
        public string TpPagamento { get; set; } = null!;
        public DateOnly DtPagamento { get; set; }
        public float VlPagamento { get; set; }
        public DateOnly DtReferenciaPagamento { get; set; }
    }
}
