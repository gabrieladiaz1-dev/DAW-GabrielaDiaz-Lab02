using Microsoft.EntityFrameworkCore;
using GestionCalidad.Data;
using GestionCalidad.Models;

namespace GestionCalidad.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly ApplicationDbContext _context;

        public InspectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inspection>> GetAllAsync()
        {
            return await _context.Inspections.OrderByDescending(i => i.FechaInspeccion).ToListAsync();
        }

        public async Task<Inspection?> GetByIdAsync(int id)
        {
            return await _context.Inspections.FindAsync(id);
        }

        public async Task CreateAsync(Inspection inspection)
        {
            _context.Inspections.Add(inspection);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Inspection inspection)
        {
            _context.Inspections.Update(inspection);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var inspection = await _context.Inspections.FindAsync(id);
            if (inspection != null)
            {
                _context.Inspections.Remove(inspection);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Inspections.AnyAsync(e => e.Id == id);
        }
    }
}
