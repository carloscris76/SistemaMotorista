using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Motorista.Entidades.DeNegocio
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Taxista")]
        [Required(ErrorMessage = "Taxista es obligatorio")]
        [Display(Name = "Taxista")]
        public int IdTaxista { get; set; }
        [Required(ErrorMessage = "Nombre es obligatorio")]
        [StringLength(60, ErrorMessage = "Maximo 60 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Apellido es obligatorio")]
        [StringLength(60, ErrorMessage = "Maximo 60 caracteres")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "TipoCliente es obligatorio")]
        public byte TipoCliente { get; set; }
        [NotMapped]
        public Taxista? Taxista { get; set; }
        [NotMapped]
        public int Top_aux { get; set; }
    }
    public enum TipoCliente
    {
        Empresarial = 1,
        Tipico = 2
    }
}
