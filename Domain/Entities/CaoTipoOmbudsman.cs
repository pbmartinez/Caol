using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoTipoOmbudsman : Entity
    {
        public CaoTipoOmbudsman()
        {
            CaoOmbudsmen = new HashSet<CaoOmbudsman>();
        }

        public sbyte Idtipo { get; set; }
        public string Descricao { get; set; } = null!;

        public virtual ICollection<CaoOmbudsman> CaoOmbudsmen { get; set; }
    }
}
