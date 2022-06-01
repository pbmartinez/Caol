using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoHelpAutor : Entity
    {
        public int CoAutor { get; set; }
        public string NoAutor { get; set; } = null!;
        public int CoFilial { get; set; }
        public string? NuDdd1 { get; set; }
        public string? NuTel1 { get; set; }
        public string? NuRamal1 { get; set; }
        public string? NuDdd2 { get; set; }
        public string? NuTel2 { get; set; }
        public string? NuRamal2 { get; set; }
        public string? DsEmail { get; set; }
        public string DsFuncao { get; set; } = null!;
    }
}
