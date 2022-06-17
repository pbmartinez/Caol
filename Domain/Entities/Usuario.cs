using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Keyless]
    public class Usuario
    {
        [Column("co_usuario")]
        public string Code { get; set; }
        [Column("no_usuario")]
        public string Name { get; set; }
    }
}
