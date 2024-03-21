using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DEMLEYCDAMMAGSMC20240321.Models
{
    public class Components
    {
        public Components()
        {
            Computers = new List<Computers>();
        }
        public int Id { get; set; }
        public int ComputerId { get; set; }

        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Name { get; set; }

        [DisplayName("Tipo")]
        [Required(ErrorMessage = "El typo es obligatorio")]
        public string Type { get; set; }

        [DisplayName("Descripcion")]
        [Required(ErrorMessage = "La descripcion es obligatorio")]
        public string Description { get; set; }
        public virtual IList<Computers> Computers { get; set; }
    }
}
