using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoValorDescanso : Entity
    {
        public int Id { get; set; }
        public string CoUsuario { get; set; } = null!;
        public string Segundos { get; set; } = null!;
        public string MesReferencia { get; set; } = null!;
    }
}
