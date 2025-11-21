namespace DAS_Grupo09_ProyectoFase2Rest.DTOs
{
    public class LoginResponse
    {
        public int Code { get; set; }
        public string? Msj { get; set; }
        public string? Token { get; set; }
        public UsuarioInfo? Usuario { get; set; }
    }

    public class UsuarioInfo
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public int IdRol { get; set; }
    }
}
