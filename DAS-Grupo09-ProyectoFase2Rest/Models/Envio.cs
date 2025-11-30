using System.ComponentModel.DataAnnotations;

namespace DAS_Grupo09_ProyectoFase2Rest.Models
{
    public class Envio
    {
        public int Id { get; set; }

        [Required]
        public int IdPaquete { get; set; }

        public int? IdConductor { get; set; }

        [Required]
        public string Origen { get; set; }

        [Required]
        public string Destino { get; set; }

        public DateTime? FechaSalida { get; set; }

        public DateTime? FechaEntrega { get; set; }

        public string? EstadoEnvio { get; set; }  // ⬅️ NULLABLE

        public string? Observaciones { get; set; }  // ⬅️ NULLABLE

        public Paquete? Paquete { get; set; }  // ⬅️ NULLABLE, SIN [Required]
    }
}