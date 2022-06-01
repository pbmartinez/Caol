using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoMovimento : Entity
    {
        public ulong CoMovimento { get; set; }
        public string CoUsuario { get; set; } = null!;
        public DateTime DtEntrada { get; set; }
        public DateTime DtSaidaAlmoco { get; set; }
        public DateTime DtVoltaAlmoco { get; set; }
        public DateTime DtSaida { get; set; }
        public byte IsEncerrado { get; set; }
    }
}
