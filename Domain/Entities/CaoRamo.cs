using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    /// <summary>
    /// Ramo de atividade de Clientes
    /// </summary>
    public partial class CaoRamo : Entity
    {
        public ulong CoRamo { get; set; }
        public string DsRamo { get; set; } = null!;
    }
}
