using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoCusto : Entity
    {
        public ulong CoCusto { get; set; }
        public sbyte CoTipoCusto { get; set; }
        public string Descricao { get; set; } = null!;
        public sbyte CoEscritorio { get; set; }
        public DateOnly? DtCompra { get; set; }
        public DateOnly? DtPagamento { get; set; }
        public string? CoBoleto { get; set; }
        public float Valor { get; set; }
        public string? Parcela { get; set; }
        public bool? Pag { get; set; }
        public ulong CoCustoHigh { get; set; }
    }
}
