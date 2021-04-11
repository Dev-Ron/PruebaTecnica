using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class User: BaseEntity
    {
        [Required(ErrorMessage = "El usuario no puede estar vacío.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña no debe estar vacía.")]
        public string Clave { get; set; }

        public string Sal { get; set; }
    }
}
