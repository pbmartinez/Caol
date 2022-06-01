using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoNoticium : Entity
    {
        public int CoNoticia { get; set; }
        public string Assunto { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string Foto { get; set; } = null!;
        public string CoUsuario { get; set; } = null!;
        public DateTime DtNoticia { get; set; }
    }
}
