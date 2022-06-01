using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoLogChamado : Entity
    {
        public int Id { get; set; }
        public int CoChamado { get; set; }
        public DateTime DtChamado { get; set; }
        public string CoUsuario { get; set; } = null!;
        public int CoDaily { get; set; }
    }
}
