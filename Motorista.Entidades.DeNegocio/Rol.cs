using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorista.Entidades.DeNegocio
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre es obligatorio")]
        [StringLength(60, ErrorMessage = "Maximo 60 caracteres")]
        public string Nombre { get; set; }
        [NotMapped]
        public int Top_aux { get; set; }
        public List<Usuario>? Usuario { get; set; }
    }
}
