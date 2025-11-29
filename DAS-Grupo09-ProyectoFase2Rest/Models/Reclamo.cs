using System.ComponentModel.DataAnnotations;

namespace DAS_Grupo09_ProyectoFase2Rest.Models
{
    public class Reclamo
    {
        public int Id { get; set; }
        public int IdEnvio { get; set; }
        public int IdCliente { get; set; }
        public string TipoReclamo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaReclamo { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public string Respuesta { get; set; }
        
        public Cliente Cliente { get; set; }
        public Envio Envio { get; set; }
    }
}
