using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoHelpMotivoChamado : Entity
    {
        public int CoMotivo { get; set; }
        public string DsMotivo { get; set; } = null!;
    }
}
