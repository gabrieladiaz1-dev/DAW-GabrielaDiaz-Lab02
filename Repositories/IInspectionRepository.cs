using GestionCalidad.Models;

namespace GestionCalidad.Repositories
{
    public interface IInspectionRepository
    {
        Task<List<Inspection>> GetAllAsync();
        Task<Inspection?> GetByIdAsync(int id);
        Task CreateAsync(Inspection inspection);
        Task UpdateAsync(Inspection inspection);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
