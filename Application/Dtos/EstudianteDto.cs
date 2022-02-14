﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Dtos
{
    public partial class EstudianteDto : Entity
    {
        [Required]
        public string Nombre { get; set; }
        [Required]
        public DateTime FechaNacimiento { get; set; }
        public int Grado { get; set; }

        // Navigation Properties
        public virtual Guid EscuelaId { get; set; }
        public virtual EscuelaDto Escuela { get; set; }

        public virtual List<MatriculaDto> Matriculas { get; set; }
    }
}
