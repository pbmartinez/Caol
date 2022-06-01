using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoOsItemMenu : Entity
    {
        public int CoItem { get; set; }
        public string DsItem { get; set; } = null!;
        public int CoSistema { get; set; }
    }
}
