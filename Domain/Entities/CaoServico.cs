using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoServico : Entity
    {
        public CaoServico()
        {
        }

        public int CoServico { get; set; }
        public string DsServico { get; set; } = null!;
    }
}
