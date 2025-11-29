using System.ComponentModel.DataAnnotations;

namespace DAS_Grupo09_ProyectoFase2.Models
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Display(Name = "Activo")]
        public bool EstaActivo { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }
    }
}
