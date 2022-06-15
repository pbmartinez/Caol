using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoFaturaPag : Entity
    {
        public int IdFaturaPag { get; set; }
        public int CoFatura { get; set; }
        public DateOnly DtEfetuado { get; set; }
        public float ValorPago { get; set; }
    }
}
