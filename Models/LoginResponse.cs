namespace DAS_Grupo09_ProyectoFase2.Models
{
    public class LoginResponse
    {
        public int code { get; set; }
        public string msj { get; set; } = string.Empty;
        public string? token { get; set; }
        public UsuarioInfo? usuario { get; set; }
    }

    public class UsuarioInfo
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public string rol { get; set; } = string.Empty;
        public int idRol { get; set; }
    }
}
