using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoBoleto : Entity
    {
        public int CoBoleto { get; set; }
        public int CoCliente { get; set; }
        public int CoSistema { get; set; }
        public int CoOs { get; set; }
        public string Valor { get; set; } = null!;
        public string Vencimento { get; set; } = null!;
        public int Status { get; set; }
        public string? Boleto { get; set; }
        public string? LinhaDig { get; set; }
        public string? Parcela { get; set; }
    }
}
