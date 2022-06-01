using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoPermissaoHoraExtra : Entity
    {
        public int IdPermissao { get; set; }
        public long CoMovimento { get; set; }
    }
}
