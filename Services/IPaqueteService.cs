using DAS_Grupo09_ProyectoFase2.Models;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public interface IPaqueteService
    {
        Task<List<Paquete>> ObtenerTodosAsync();
        Task<Paquete> ObtenerPorIdAsync(int id);
        Task<Paquete> ObtenerPorCodigoAsync(string codigo);
        Task<bool> CrearAsync(Paquete paquete);
        Task<bool> ActualizarAsync(Paquete paquete);
        Task<bool> EliminarAsync(int id);
    }
}
