using System.ComponentModel.DataAnnotations;

namespace DAS_Grupo09_ProyectoFase2Rest.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La clave es requerida")]
        public string Clave { get; set; } = string.Empty;
    }
}
