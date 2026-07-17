using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionCalidad.ViewModels
{
    public class InspectionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Display(Name = "Producto")]
        public string Producto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El inspector es obligatorio.")]
        [Display(Name = "Inspector")]
        public string Inspector { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inspección es obligatoria.")]
        [Display(Name = "Fecha de Inspección")]
        [DataType(DataType.Date)]
        public DateTime FechaInspeccion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "Las observaciones son obligatorias.")]
        [Display(Name = "Observaciones")]
        [MinLength(10, ErrorMessage = "Las observaciones deben tener entre 10 y 500 caracteres.")]
        [MaxLength(500, ErrorMessage = "Las observaciones deben tener entre 10 y 500 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; } = string.Empty;

        public List<SelectListItem>? ProductosList { get; set; }
        public List<SelectListItem>? InspectoresList { get; set; }
        public List<SelectListItem>? EstadosList { get; set; }
    }
}
