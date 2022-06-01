using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoHelpChamado : Entity
    {
        public int CoChamado { get; set; }
        public string DsChamado { get; set; } = null!;
        public string? DsSolucao { get; set; }
        public sbyte CoStatus { get; set; }
        public int CoMotivo { get; set; }
        public int CoTela { get; set; }
        public int CoAutor { get; set; }
        public int CoFilial { get; set; }
        public long CoSistema { get; set; }
        public DateOnly DtChamado { get; set; }
        public DateOnly? DtSolucao { get; set; }
    }
}
