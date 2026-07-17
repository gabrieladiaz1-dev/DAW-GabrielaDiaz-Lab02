using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionCalidad.Repositories;
using GestionCalidad.ViewModels;
using GestionCalidad.Models;

namespace GestionCalidad.Controllers
{
    public class InspectionsController : Controller
    {
        private readonly IInspectionRepository _repository;

        public InspectionsController(IInspectionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var inspections = await _repository.GetAllAsync();
            return View(inspections);
        }

        public async Task<IActionResult> Details(int id)
        {
            var inspection = await _repository.GetByIdAsync(id);
            if (inspection == null)
            {
                return NotFound();
            }
            return View(inspection);
        }

        public IActionResult Create()
        {
            var viewModel = new InspectionViewModel
            {
                FechaInspeccion = DateTime.Today,
                ProductosList = GetProductosList(),
                InspectoresList = GetInspectoresList(),
                EstadosList = GetEstadosList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InspectionViewModel viewModel)
        {
            ValidateSelectLength(viewModel, ModelState);

            if (ModelState.IsValid)
            {
                try
                {
                    var inspection = new Inspection
                    {
                        Producto = viewModel.Producto,
                        Inspector = viewModel.Inspector,
                        FechaInspeccion = viewModel.FechaInspeccion,
                        Estado = viewModel.Estado,
                        Observaciones = viewModel.Observaciones
                    };

                    await _repository.CreateAsync(inspection);
                    TempData["Success"] = "Inspección creada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar la inspección: " + ex.Message);
                }
            }

            viewModel.ProductosList = GetProductosList();
            viewModel.InspectoresList = GetInspectoresList();
            viewModel.EstadosList = GetEstadosList();
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var inspection = await _repository.GetByIdAsync(id);
            if (inspection == null)
            {
                return NotFound();
            }

            var viewModel = new InspectionViewModel
            {
                Id = inspection.Id,
                Producto = inspection.Producto,
                Inspector = inspection.Inspector,
                FechaInspeccion = inspection.FechaInspeccion,
                Estado = inspection.Estado,
                Observaciones = inspection.Observaciones,
                ProductosList = GetProductosList(),
                InspectoresList = GetInspectoresList(),
                EstadosList = GetEstadosList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InspectionViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            ValidateSelectLength(viewModel, ModelState);

            if (ModelState.IsValid)
            {
                try
                {
                    var inspection = await _repository.GetByIdAsync(id);
                    if (inspection == null)
                    {
                        return NotFound();
                    }

                    inspection.Producto = viewModel.Producto;
                    inspection.Inspector = viewModel.Inspector;
                    inspection.FechaInspeccion = viewModel.FechaInspeccion;
                    inspection.Estado = viewModel.Estado;
                    inspection.Observaciones = viewModel.Observaciones;

                    await _repository.UpdateAsync(inspection);
                    TempData["Success"] = "Inspección actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar la inspección: " + ex.Message);
                }
            }

            viewModel.ProductosList = GetProductosList();
            viewModel.InspectoresList = GetInspectoresList();
            viewModel.EstadosList = GetEstadosList();
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var inspection = await _repository.GetByIdAsync(id);
            if (inspection == null)
            {
                return NotFound();
            }
            return View(inspection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            TempData["Success"] = "Inspección eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Dashboard()
        {
            var inspections = await _repository.GetAllAsync();

            var total = inspections.Count;
            var aprobadas = inspections.Count(i => i.Estado == "Aprobado");
            var rechazadas = inspections.Count(i => i.Estado == "Rechazado");
            var pendientes = inspections.Count(i => i.Estado == "Pendiente");

            var viewModel = new DashboardViewModel
            {
                TotalInspecciones = total,
                Aprobadas = aprobadas,
                Rechazadas = rechazadas,
                Pendientes = pendientes,
                PorcentajeAprobacion = total > 0 ? Math.Round((double)aprobadas / total * 100, 1) : 0,
                PorcentajeRechazo = total > 0 ? Math.Round((double)rechazadas / total * 100, 1) : 0,
                PorcentajePendiente = total > 0 ? Math.Round((double)pendientes / total * 100, 1) : 0
            };

            return View(viewModel);
        }

        private static void ValidateSelectLength(InspectionViewModel viewModel, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            if (viewModel.Producto != null && (viewModel.Producto.Length < 3 || viewModel.Producto.Length > 100))
            {
                modelState.AddModelError("Producto", "El producto debe tener entre 3 y 100 caracteres.");
            }
            if (viewModel.Inspector != null && (viewModel.Inspector.Length < 3 || viewModel.Inspector.Length > 100))
            {
                modelState.AddModelError("Inspector", "El inspector debe tener entre 3 y 100 caracteres.");
            }
        }

        private List<SelectListItem> GetProductosList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Producto A", Text = "Producto A" },
                new SelectListItem { Value = "Producto B", Text = "Producto B" },
                new SelectListItem { Value = "Producto C", Text = "Producto C" },
                new SelectListItem { Value = "Producto D", Text = "Producto D" }
            };
        }

        private List<SelectListItem> GetInspectoresList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Gabriela Diaz", Text = "Gabriela Diaz" },
                new SelectListItem { Value = "Victor Sierra", Text = "Victor Sierra" },
                new SelectListItem { Value = "Fatima Diaz", Text = "Fatima Diaz" },
                new SelectListItem { Value = "Alejandro Rivera", Text = "Alejandro Rivera" }
            };
        }

        private List<SelectListItem> GetEstadosList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "Aprobado", Text = "Aprobado" },
                new SelectListItem { Value = "Rechazado", Text = "Rechazado" },
                new SelectListItem { Value = "Pendiente", Text = "Pendiente" }
            };
        }
    }
}
