using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoServico : Entity
    {
        public CaoServico()
        {
            CaoFaturas = new HashSet<CaoFatura>();
        }

        public int CoServico { get; set; }
        public string DsServico { get; set; } = null!;

        public virtual ICollection<CaoFatura> CaoFaturas { get; set; }
    }
}
