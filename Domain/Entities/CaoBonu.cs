using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoBonu : Entity
    {
        public int BonCategoria { get; set; }
        public int BonInicio { get; set; }
        public int BonFim { get; set; }
        public float? BonValorSem { get; set; }
        public float? BonValorFimsem { get; set; }
    }
}
