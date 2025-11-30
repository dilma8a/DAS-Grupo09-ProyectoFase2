using System.ComponentModel.DataAnnotations;

namespace DAS_Grupo09_ProyectoFase2.Models
{
    public class Paquete
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código de barra es obligatorio")]
        [Display(Name = "Código de Barra")]
        public string CodigoBarra { get; set; }

        [Required(ErrorMessage = "El peso es obligatorio")]
        [Range(0.01, 1000, ErrorMessage = "El peso debe estar entre 0.01 y 1000 kg")]
        [Display(Name = "Peso (kg)")]
        public decimal Peso { get; set; }

        [Display(Name = "Dimensiones")]
        public string? Dimensiones { get; set; }  // ⬅️ NULLABLE

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }  // ⬅️ NULLABLE

        [Display(Name = "Estado Actual")]
        public string? EstadoActual { get; set; }  // ⬅️ NULLABLE (se asigna en controller)

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        // Propiedad de navegación - NO REQUERIDA
        public Cliente? Cliente { get; set; }  // ⬅️ NULLABLE
    }
}