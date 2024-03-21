using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DEMLEYCDAMMAGSMC20240321.Models
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 32 caracteres.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede tener más de 100 caracteres.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Range(0, 255, ErrorMessage = "El estado debe ser un número entre 0 y 255.")]
        public byte Status { get; set; }

        public byte[] Image { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int Rol { get; set; }
    }
}
