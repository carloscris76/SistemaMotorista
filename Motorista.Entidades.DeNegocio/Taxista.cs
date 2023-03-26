using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorista.Entidades.DeNegocio
{
    public class Taxista
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre es obligatorio")]
        [StringLength(60, ErrorMessage = "Maximo 60 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Apellido es obligatorio")]
        [StringLength(60, ErrorMessage = "Maximo 60 caracteres")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Placa es obligatorio")]
        [StringLength(60, ErrorMessage = "Maximo 60 caracteres")]
        public string Placa { get; set; }
        [Required(ErrorMessage = "Color es obligatorio")]
        public byte Color { get; set; }
        [Required(ErrorMessage = "Marca es obligatorio")]
        public byte Marca { get; set; }
        [Required(ErrorMessage = "Estatus es obligatorio")]
        public byte Estatus { get; set; }
        [Required(ErrorMessage = "Licencia es obligatorio")]
        public int Licencia { get; set; }
        [Required(ErrorMessage = "Nacionalidad es obligatorio")]
        public byte Nacionalidad { get; set; }
        [Required(ErrorMessage = "Estado es obligatorio")]
        public byte Estado { get; set; }
        [NotMapped]
        public int Top_Aux { get; set; }
        public List<Cliente>? Cliente { get; set; }
    }
    public enum Color
    {
        Rojo = 1,
        Negro = 2,
        Azul = 3,
        Blanco = 4,
        Gris = 5,
        Amarillo = 6
    }
    public enum Marca
    {
        Toyota = 1,
        Hyundai = 3,
        Nissan = 4
    }
    public enum Estatus
    {
        ACTIVO = 1,
        INACTIVO = 2
    }
    public enum Nacionalidad
    {
        Salvadoreño = 1
    }
    public enum Estado
    {
        Nuevo = 1,
        Usado = 2
    }
}
