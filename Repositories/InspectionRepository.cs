using System.Text.Json;
using GestionCalidad.Models;

namespace GestionCalidad.Repositories
{
    public class InspectionRepository : IInspectionRepository
    {
        private readonly string _filePath;
        private List<Inspection> _inspections;
        private int _nextId;
        private readonly object _lock = new();

        public InspectionRepository()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "inspecciones.json");
            _inspections = new List<Inspection>();
            _nextId = 1;
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    var json = File.ReadAllText(_filePath);
                    var data = JsonSerializer.Deserialize<List<Inspection>>(json);
                    if (data != null && data.Count > 0)
                    {
                        _inspections = data;
                        _nextId = _inspections.Max(i => i.Id) + 1;
                    }
                }
                catch
                {
                    _inspections = new List<Inspection>();
                    _nextId = 1;
                }
            }
        }

        private void SaveToFile()
        {
            var json = JsonSerializer.Serialize(_inspections, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public Task<List<Inspection>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_inspections.OrderByDescending(i => i.FechaInspeccion).ToList());
            }
        }

        public Task<Inspection?> GetByIdAsync(int id)
        {
            lock (_lock)
            {
                return Task.FromResult(_inspections.FirstOrDefault(i => i.Id == id));
            }
        }

        public Task CreateAsync(Inspection inspection)
        {
            lock (_lock)
            {
                inspection.Id = _nextId++;
                _inspections.Add(inspection);
                SaveToFile();
            }
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Inspection inspection)
        {
            lock (_lock)
            {
                var existing = _inspections.FirstOrDefault(i => i.Id == inspection.Id);
                if (existing != null)
                {
                    existing.Producto = inspection.Producto;
                    existing.Inspector = inspection.Inspector;
                    existing.FechaInspeccion = inspection.FechaInspeccion;
                    existing.Estado = inspection.Estado;
                    existing.Observaciones = inspection.Observaciones;
                    SaveToFile();
                }
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            lock (_lock)
            {
                _inspections.RemoveAll(i => i.Id == id);
                SaveToFile();
            }
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id)
        {
            lock (_lock)
            {
                return Task.FromResult(_inspections.Any(i => i.Id == id));
            }
        }
    }
}
