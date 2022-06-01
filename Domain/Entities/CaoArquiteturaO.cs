using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoArquiteturaO : Entity
    {
        public long CoArquitetura { get; set; }
        public string DsArquitetura { get; set; } = null!;
    }
}
