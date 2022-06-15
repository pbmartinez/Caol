using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoHelpStatusChamado : Entity
    {
        public int CoStatus { get; set; }
        public string DsStatus { get; set; } = null!;
    }
}
