using System.ComponentModel.DataAnnotations;

namespace GestionCalidad.Models
{
    public class Inspection
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio.")]
        [Display(Name = "Producto")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El producto debe tener entre 3 y 100 caracteres.")]
        public string Producto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El inspector es obligatorio.")]
        [Display(Name = "Inspector")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El inspector debe tener entre 3 y 100 caracteres.")]
        public string Inspector { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inspección es obligatoria.")]
        [Display(Name = "Fecha de Inspección")]
        [DataType(DataType.Date)]
        [FechaNoFutura(ErrorMessage = "La fecha no puede ser futura.")]
        public DateTime FechaInspeccion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "Las observaciones son obligatorias.")]
        [Display(Name = "Observaciones")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Las observaciones deben tener entre 10 y 500 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; } = string.Empty;
    }

    public class FechaNoFuturaAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                return date.Date <= DateTime.Today;
            }
            return true;
        }
    }
}
