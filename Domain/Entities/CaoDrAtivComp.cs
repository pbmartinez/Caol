using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoDrAtivComp : Entity
    {
        public int IdAtivComp { get; set; }
        public string CoUsuario { get; set; } = null!;
        public DateOnly Data { get; set; }
        public string Assunto { get; set; } = null!;
        public TimeOnly TempoGasto { get; set; }
    }
}
