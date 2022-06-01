using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoOmbudsman : Entity
    {
        public int Id { get; set; }
        public sbyte Idtipo { get; set; }
        public sbyte Idcategoria { get; set; }
        public DateTime Data { get; set; }
        public string Comentario { get; set; } = null!;
        public sbyte CoEscritorio { get; set; }

        public virtual CaoCategoriasOmbudsman IdcategoriaNavigation { get; set; } = null!;
        public virtual CaoTipoOmbudsman IdtipoNavigation { get; set; } = null!;
    }
}
