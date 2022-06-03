using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Dtos
{
    public partial class CaoSalarioDto : Entity
    {
        public string CoUsuario { get; set; } = null!;
        public DateOnly DtAlteracao { get; set; }
        public float BrutSalario { get; set; }
        public float LiqSalario { get; set; }

        public virtual CaoUsuarioDto CaoUsuario { get; set; }
    }
}
