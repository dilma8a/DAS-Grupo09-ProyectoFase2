using DAS_Grupo09_ProyectoFase2.Models;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public interface IClienteService
    {
        Task<List<Cliente>> GetClientesAsync();
        Task<Cliente?> GetClienteByIdAsync(int id);
        Task<Cliente?> CreateClienteAsync(Cliente cliente);
        Task<bool> UpdateClienteAsync(int id, Cliente cliente);
        Task<bool> DeleteClienteAsync(int id);
    }
}