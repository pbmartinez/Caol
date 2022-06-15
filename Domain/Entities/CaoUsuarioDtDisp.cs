using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoUsuarioDtDisp : Entity
    {
        public uint CoDtDisp { get; set; }
        public string CoUsuario { get; set; }
        public DateOnly DtDisp { get; set; }
        public DateOnly DtAlt { get; set; }
    }
}
