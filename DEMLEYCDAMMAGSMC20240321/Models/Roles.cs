using System.ComponentModel.DataAnnotations;

namespace DEMLEYCDAMMAGSMC20240321.Models
{
    public class Roles
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre debe tener como máximo 50 caracteres.")]
        public string? Name { get; set; }

        [StringLength(50, ErrorMessage = "La descripción debe tener como máximo 50 caracteres.")]
        public string? Description { get; set; }
    }

}

