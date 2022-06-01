using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoOsFase : Entity
    {
        public int CoFase { get; set; }
        public string? Descricao { get; set; }
        public string DescricaoIngl { get; set; } = null!;
        public int? Ordem { get; set; }
    }
}
