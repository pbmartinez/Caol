using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class CaoRelEmailO : Entity
    {
        public int Id { get; set; }
        public int CoEmail { get; set; }
        public int CoOs { get; set; }
    }
}
