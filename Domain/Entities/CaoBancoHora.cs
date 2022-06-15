using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// Banco de horas dos usuários...
    /// </summary>
    public partial class CaoBancoHora : Entity
    {
        public uint CoBancHoras { get; set; }
        public string CoUsuario { get; set; } = null!;
        public string Periodo { get; set; } = null!;
        public int MinMes { get; set; }
        public int MinFerias { get; set; }
        public int MinPago { get; set; }
        public int MinTotal { get; set; }
    }
}
