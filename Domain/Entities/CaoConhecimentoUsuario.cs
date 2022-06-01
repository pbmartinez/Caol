using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoConhecimentoUsuario : Entity
    {
        public string CoUsuario { get; set; } = null!;
        public uint CoConhecimento { get; set; }
        public uint? NvConhecimento { get; set; }
        public byte? IsCertificado { get; set; }
    }
}
