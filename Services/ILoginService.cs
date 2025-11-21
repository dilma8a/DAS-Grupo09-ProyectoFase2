using DAS_Grupo09_ProyectoFase2.Models;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public interface ILoginService
    {
        Task<LoginResponse> LoginAsync(string usuario, string clave);
    }
}
