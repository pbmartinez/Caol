using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoParticipacaoFuncionario : Entity
    {
        public uint CoPartFuncionario { get; set; }
        public float PcParticipacao { get; set; }
        public string CoUsuario { get; set; } = null!;
        public byte CoEscritorio { get; set; }
        public DateOnly DtReferencia { get; set; }
    }
}
