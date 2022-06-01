using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoSalario : Entity
    {
        public string CoUsuario { get; set; } = null!;
        public DateOnly DtAlteracao { get; set; }
        public float BrutSalario { get; set; }
        public float LiqSalario { get; set; }
    }
}
