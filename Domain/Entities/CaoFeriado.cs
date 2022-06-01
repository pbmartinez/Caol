using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoFeriado : Entity
    {
        public byte? Dia { get; set; }
        public byte? Mes { get; set; }
        public int? Ano { get; set; }
    }
}
