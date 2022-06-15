using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoHorarioAlmoco : Entity
    {
        public string CoUsuario { get; set; } = null!;
        public string AlmocoSaidaHora { get; set; } = null!;
        public string AlmocoVoltaHora { get; set; } = null!;
    }
}
