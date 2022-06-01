using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoOsPrazo : Entity
    {
        public int CoPrazo { get; set; }
        public int? CoOs { get; set; }
        public uint? CoFase { get; set; }
        public int? TotalAnalista { get; set; }
        public int? TotalHora { get; set; }
    }
}
