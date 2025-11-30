using System.ComponentModel.DataAnnotations;

namespace DAS_Grupo09_ProyectoFase2Rest.Models
{
    public class Paquete
    {
        public int Id { get; set; }

        [Required]
        public string CodigoBarra { get; set; }

        [Required]
        public decimal Peso { get; set; }

        public string? Dimensiones { get; set; }  // ⬅️ NULLABLE

        public string? Descripcion { get; set; }  // ⬅️ NULLABLE

        public string? EstadoActual { get; set; }  // ⬅️ NULLABLE

        public DateTime FechaRegistro { get; set; }

        [Required]
        public int IdCliente { get; set; }

        public Cliente? Cliente { get; set; }  // ⬅️ NULLABLE, SIN [Required]
    }
}