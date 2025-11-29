using System.ComponentModel.DataAnnotations;

namespace DAS_Grupo09_ProyectoFase2.Models
{
    // Modelo para los filtros de reportes
    public class FiltroReporte
    {
        [Display(Name = "Fecha Inicio")]
        [DataType(DataType.Date)]
        public DateTime? FechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        [Display(Name = "Estado")]
        public string? Estado { get; set; }
    }

    // Modelo para estadísticas generales
    public class EstadisticasGenerales
    {
        public int TotalEnvios { get; set; }
        public int TotalClientes { get; set; }
        public int TotalPaquetes { get; set; }
        public int TotalReclamos { get; set; }

        public int EnviosEnTransito { get; set; }
        public int EnviosEntregados { get; set; }
        public int EnviosPendientes { get; set; }

        public int ReclamosPendientes { get; set; }
        public int ReclamosResueltos { get; set; }
    }

    // Modelo para reporte de envíos
    public class ReporteEnvio
    {
        public int Id { get; set; }
        public string CodigoSeguimiento { get; set; }
        public string ClienteNombre { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Estado { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public int DiasTranscurridos { get; set; }
    }

    // Modelo para reporte de reclamos
    public class ReporteReclamo
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; }
        public int EnvioId { get; set; }
        public string TipoReclamo { get; set; }
        public string Estado { get; set; }
        public DateTime FechaReclamo { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public int DiasResolucion { get; set; }
    }

    // Modelo para estadísticas por estado
    public class EstadisticaPorEstado
    {
        public string Estado { get; set; }
        public int Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
    }
}