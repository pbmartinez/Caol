using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// Descrição da Visita
    /// </summary>
    public partial class CaoComplemento : Entity
    {
        public ulong CoComplemento { get; set; }
        public string DsComplemento { get; set; }
    }
}
