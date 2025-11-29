using System.ComponentModel.DataAnnotations;

namespace DAS_Grupo09_ProyectoFase2.Models
{
    public class Reclamo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de envío es requerido")]
        [Display(Name = "ID de Envío")]
        public int IdEnvio { get; set; }

        [Required(ErrorMessage = "El cliente es requerido")]
        [Display(Name = "ID de Cliente")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El tipo de reclamo es requerido")]
        [Display(Name = "Tipo de Reclamo")]
        [StringLength(50)]
        public string TipoReclamo { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [Display(Name = "Descripción")]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Display(Name = "Estado")]
        [StringLength(20)]
        public string Estado { get; set; }

        [Display(Name = "Fecha de Reclamo")]
        public DateTime FechaReclamo { get; set; }

        [Display(Name = "Fecha de Resolución")]
        public DateTime? FechaResolucion { get; set; }

        [Display(Name = "Respuesta")]
        [StringLength(500)]
        public string Respuesta { get; set; }

        // Relaciones
        public Envio Envio { get; set; }
        public Cliente Cliente { get; set; }
    }
}
