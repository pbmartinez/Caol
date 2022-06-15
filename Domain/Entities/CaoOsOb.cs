using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoOsOb : Entity
    {
        public int CoObs { get; set; }
        public int? CoOs { get; set; }
        public string CoUsuario { get; set; }
        public string Descricao { get; set; }
        public DateTime? DtObs { get; set; }
    }
}
