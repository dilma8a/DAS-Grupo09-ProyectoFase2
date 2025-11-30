using System.ComponentModel.DataAnnotations;

namespace DAS_Grupo09_ProyectoFase2.Models
{
    public class Envio
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un paquete")]
        [Display(Name = "Paquete")]
        public int IdPaquete { get; set; }

        [Display(Name = "Conductor")]
        public int? IdConductor { get; set; }

        [Required(ErrorMessage = "El origen es obligatorio")]
        [Display(Name = "Origen")]
        public string Origen { get; set; }

        [Required(ErrorMessage = "El destino es obligatorio")]
        [Display(Name = "Destino")]
        public string Destino { get; set; }

        [Display(Name = "Fecha de Salida")]
        public DateTime? FechaSalida { get; set; }

        [Display(Name = "Fecha de Entrega")]
        public DateTime? FechaEntrega { get; set; }

        [Display(Name = "Estado del Envío")]
        public string? EstadoEnvio { get; set; }  // ⬅️ NULLABLE

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }  // ⬅️ NULLABLE

        // Propiedades de navegacion - NULLABLE
        public Paquete? Paquete { get; set; }  // ⬅️ NULLABLE
        public Empleado? Conductor { get; set; }  // ⬅️ NULLABLE
    }
}