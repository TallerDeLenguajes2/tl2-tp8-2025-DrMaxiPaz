using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering; // Necesario para SelectList
namespace ViewModels
{
    public class PresupuestoViewModel
    {
        public int IdPresupuesto { get; set; }
        // Validación: Requerido
        [Display(Name = "Nombre del Destinatario")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        // Opcional: Se puede añadir la validación de formato de email si se opta por guardar el mail.
        // [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string NombreDestinatario { get; set; }
        // Validación: Requerido y tipo de dato
        [Display(Name = "Fecha de Creación")]
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [DataType(DataType.Date)]
        public DateOnly FechaCreacion { get; set; }
    
        public SelectList ListaProductos { get; set; }

    }
}