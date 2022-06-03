using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class CaoSalario : Entity
    {
        public string CoUsuario { get; set; } = null!;
        public DateOnly DtAlteracao { get; set; }
        public float BrutSalario { get; set; }
        public float LiqSalario { get; set; }

        [ForeignKey("CoUsuario")]
        public virtual CaoUsuario CaoUsuario { get; set; }
    }
}
