using Microsoft.EntityFrameworkCore;
using Motorista.Entidades.DeNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorista.AccesoADatos
{
    public class TaxistaDAL
    {
        public static async Task<int> CrearAsync(Taxista pTaxista)
        {
            int result = 0;
            using (var bdContexto = new BDContexto())
            {
                bdContexto.Add(pTaxista);
                result = await bdContexto.SaveChangesAsync();
            }
            return result;
        }
        public static async Task<int> ModificarAsync(Taxista pTaxista)
        {
            int result = 0;
            using (var bdContexto = new BDContexto())
            {
                var taxista = await bdContexto.Taxista.FirstOrDefaultAsync(s => s.Id == pTaxista.Id);
                taxista.Nombre = pTaxista.Nombre;
                taxista.Apellido = pTaxista.Apellido;
                taxista.Placa = pTaxista.Placa;
                taxista.Color = pTaxista.Color;
                taxista.Marca = pTaxista.Marca;
                taxista.Estatus = pTaxista.Estatus;
                taxista.Licencia = pTaxista.Licencia;
                taxista.Estado = pTaxista.Estado;
                bdContexto.Update(taxista);
                result = await bdContexto.SaveChangesAsync();
            }
            return result;
        }
        public static async Task<int> EliminarAsync(Taxista pTaxista)
        {
            int result = 0;
            using (var bdContexto = new BDContexto())
            {
                var taxista = await bdContexto.Taxista.FirstOrDefaultAsync(s => s.Id == pTaxista.Id);
                bdContexto.Taxista.Remove(taxista);
                result = await bdContexto.SaveChangesAsync();
            }
            return result;
        }
        public static async Task<Taxista> ObtenerPorIdAsync(Taxista pTaxista)
        {
            var taxista = new Taxista();
            using (var bdContexto = new BDContexto())
            {
                taxista = await bdContexto.Taxista.FirstOrDefaultAsync(s => s.Id == pTaxista.Id);
            }
            return taxista;
        }
        public static async Task<List<Taxista>> ObtenerTodosAsync()
        {
            var taxistas = new List<Taxista>();
            using (var bdContexto = new BDContexto())
            {
                taxistas = await bdContexto.Taxista.ToListAsync();
            }
            return taxistas;
        }
        internal static IQueryable<Taxista> QuerySelect(IQueryable<Taxista> pQuery, Taxista pTaxista)
        {
            if (pTaxista.Id > 0)
                pQuery = pQuery.Where(s => s.Id == pTaxista.Id);
            if (!string.IsNullOrWhiteSpace(pTaxista.Nombre))
                pQuery = pQuery.Where(s => s.Nombre.Contains(pTaxista.Nombre));
            if (!string.IsNullOrWhiteSpace(pTaxista.Apellido))
                pQuery = pQuery.Where(s => s.Apellido.Contains(pTaxista.Apellido));
            if (!string.IsNullOrWhiteSpace(pTaxista.Placa))
                pQuery = pQuery.Where(s => s.Placa.Contains(pTaxista.Placa));
            if (pTaxista.Color > 0)
                pQuery = pQuery.Where(s => s.Color == pTaxista.Color);
            if (pTaxista.Marca > 0)
                pQuery = pQuery.Where(s => s.Marca == pTaxista.Marca);
            if (pTaxista.Estatus > 0)
                pQuery = pQuery.Where(s => s.Estatus == pTaxista.Estatus);
            if (pTaxista.Licencia > 0)
                pQuery = pQuery.Where(s => s.Licencia == pTaxista.Licencia);
            if (pTaxista.Nacionalidad > 0)
                pQuery = pQuery.Where(s => s.Nacionalidad == pTaxista.Nacionalidad);
            if (pTaxista.Estado > 0)
                pQuery = pQuery.Where(s => s.Estado == pTaxista.Estado);
            pQuery = pQuery.OrderByDescending(s => s.Id).AsQueryable();
            if (pTaxista.Top_Aux > 0)
                pQuery = pQuery.Take(pTaxista.Top_Aux).AsQueryable();
            return pQuery;
        }
        public static async Task<List<Taxista>> BuscarAsync(Taxista pTaxista)
        {
            var taxistas = new List<Taxista>();
            using (var bdContexto = new BDContexto())
            {
                var select = bdContexto.Taxista.AsQueryable();
                select = QuerySelect(select, pTaxista);
                taxistas = await select.ToListAsync();
            }
            return taxistas;
        }
    }
}
