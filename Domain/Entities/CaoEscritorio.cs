using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoEscritorio : Entity
    {
        public byte CoEscritorio { get; set; }
        public string Local { get; set; } = null!;
    }
}
