using Microsoft.EntityFrameworkCore;
using Motorista.Entidades.DeNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorista.AccesoADatos
{
    public class ClienteDAL
    {
        public static async Task<int> CrearAsync(Cliente pCliente)
        {
            int result = 0;
            using (var bdContexto = new BDContexto())
            {
                bdContexto.Add(pCliente);
                result = await bdContexto.SaveChangesAsync();
            }
            return result;
        }
        public static async Task<int> ModificarAsync(Cliente pCliente)
        {
            int result = 0;
            using (var bdContexto = new BDContexto())
            {
                var cliente = await bdContexto.Cliente.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
                cliente.IdTaxista = pCliente.IdTaxista;
                cliente.Nombre = pCliente.Nombre;
                cliente.Apellido = pCliente.Apellido;
                cliente.TipoCliente = pCliente.TipoCliente;
                bdContexto.Update(cliente);
                result = await bdContexto.SaveChangesAsync();
            }
            return result;
        }
        public static async Task<int> EliminarAsync(Cliente pCliente)
        {
            int result = 0;
            using (var bdContexto = new BDContexto())
            {
                var cliente = await bdContexto.Cliente.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
                bdContexto.Cliente.Remove(cliente);
                result = await bdContexto.SaveChangesAsync();
            }
            return result;
        }
        public static async Task<Cliente> ObtenerPorIdAsync(Cliente pCliente)
        {
            var cliente = new Cliente();
            using (var bdContexto = new BDContexto())
            {
                cliente = await bdContexto.Cliente.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            }
            return cliente;
        }
        public static async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var clientes = new List<Cliente>();
            using (var bdContexto = new BDContexto())
            {
                clientes = await bdContexto.Cliente.ToListAsync();
            }
            return clientes;
        }
        internal static IQueryable<Cliente> QuerySelect(IQueryable<Cliente> pQuery, Cliente pCliente)
        {
            if (pCliente.Id > 0)
                pQuery = pQuery.Where(s => s.Id == pCliente.Id);
            if (pCliente.IdTaxista > 0)
                pQuery = pQuery.Where(s => s.IdTaxista == pCliente.IdTaxista);
            if (!string.IsNullOrWhiteSpace(pCliente.Nombre))
                pQuery = pQuery.Where(s => s.Nombre.Contains(pCliente.Nombre));
            if (!string.IsNullOrWhiteSpace(pCliente.Apellido))
                pQuery = pQuery.Where(s => s.Apellido.Contains(pCliente.Apellido));
            if (pCliente.TipoCliente >0)
                pQuery = pQuery.Where(s => s.TipoCliente == pCliente.TipoCliente);
            pQuery = pQuery.OrderByDescending(s => s.Id).AsQueryable();
            if (pCliente.Top_aux > 0)
                pQuery = pQuery.Take(pCliente.Top_aux).AsQueryable();
            return pQuery;
        }
        public static async Task<List<Cliente>> BuscarAsync(Cliente pCliente)
        {
            var clientes = new List<Cliente>();
            using (var bdContexto = new BDContexto())
            {
                var select = bdContexto.Cliente.AsQueryable();
                select = QuerySelect(select, pCliente);
                clientes = await select.ToListAsync();
            }
            return clientes;
        }
        public static async Task<List<Cliente>> BuscarIncluirTaxistaAsync(Cliente pCliente)
        {
            var clientes = new List<Cliente>();
            using (var bdContexto = new BDContexto())
            {
                var select = bdContexto.Cliente.AsQueryable();
                select = QuerySelect(select, pCliente).Include(s => s.Taxista).AsQueryable();
                clientes = await select.ToListAsync();
            }
            return clientes;
        }
    }
}
