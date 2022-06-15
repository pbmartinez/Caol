using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoCategoriasOmbudsman : Entity
    {
        public CaoCategoriasOmbudsman()
        {
            CaoOmbudsmen = new HashSet<CaoOmbudsman>();
        }

        public sbyte Idcategoria { get; set; }
        public string Descricao { get; set; } = null!;

        public virtual ICollection<CaoOmbudsman> CaoOmbudsmen { get; set; }
    }
}
