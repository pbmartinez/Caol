﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Curso : Entity
    {
        public Curso()
        {
            Matriculas = new List<Matricula>();
        }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }


        //Navigation Properties
        [Required]
        public virtual Guid EscuelaId { get; set; }
        public virtual Escuela Escuela { get; set; }

        public virtual List<Matricula> Matriculas { get; set; }
    }
}