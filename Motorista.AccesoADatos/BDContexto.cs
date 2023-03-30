using Microsoft.EntityFrameworkCore;
using Motorista.Entidades.DeNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorista.AccesoADatos
{
    public class BDContexto: DbContext
    {
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Taxista> Taxista { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=DESKTOP-R0RFR5B;Initial Catalog=Motoristadb;Integrated Security=True");
        }
    }
}
