using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DEMLEYCDAMMAGSMC20240321.Models
{
    public class Computers
    {
        public int Id { get; set; }
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; }
        [DisplayName("Marca")]
        [Required(ErrorMessage = "La descripcion es obligatoria.")]
        public string Brand {  get; set; }

        public virtual IList<Components> Components { get; set; } // añadimos la relación

    }
}
