using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoHistOcorrenciasO : Entity
    {
        public int Idocorrencia { get; set; }
        public int? CoOs { get; set; }
        public string CoUsuario { get; set; }
        public DateTime? Data { get; set; }
        public string Tipo { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string Responsavel { get; set; } = null!;
        public DateTime? DataFechamento { get; set; }

        public virtual CaoO CoOsNavigation { get; set; }
        public virtual CaoUsuario CoUsuarioNavigation { get; set; }
    }
}
