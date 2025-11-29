namespace DAS_Grupo09_ProyectoFase2Rest.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public bool EstaActivo { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
