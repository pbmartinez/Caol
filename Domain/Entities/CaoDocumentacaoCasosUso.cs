using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoDocumentacaoCasosUso : Entity
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public int CoSistema { get; set; }
    }
}
