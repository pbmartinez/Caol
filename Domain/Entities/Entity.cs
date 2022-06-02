using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Entity
    {
        public virtual string Id { get; set; }



        public bool IsTransient => string.IsNullOrEmpty(Id) || string.IsNullOrWhiteSpace(Id) || Id == "0";



        public void GenerateIdentity()
        {
            //if (IsTransient)
            //    Id = Guid.NewGuid();
        }

    }
}
