using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoOsAnalistum : Entity
    {
        public int CoAnalista { get; set; }
        public int? CoOs { get; set; }
        public string CoUsuario { get; set; }
    }
}
