using Motorista.AccesoADatos;
using Motorista.Entidades.DeNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Motorista.LogicaDeNegocio
{
    public class TaxistaBL
    {
        public async Task<int> CrearAsync(Taxista pTaxista)
        {
            return await TaxistaDAL.CrearAsync(pTaxista);
        }
        public async Task<int> ModificarAsync(Taxista pTaxista)
        {
            return await TaxistaDAL.ModificarAsync(pTaxista);
        }
        public async Task<int> EliminarAsync(Taxista pTaxista)
        {
            return await TaxistaDAL.EliminarAsync(pTaxista);
        }
        public async Task<Taxista> ObtenerPorIdAsync(Taxista pTaxista)
        {
            return await TaxistaDAL.ObtenerPorIdAsync(pTaxista);
        }
        public async Task<List<Taxista>> ObtenerTodosAsync()
        {
            return await TaxistaDAL.ObtenerTodosAsync();
        }
        public async Task<List<Taxista>> BuscarAsync(Taxista pTaxista)
        {
            return await TaxistaDAL.BuscarAsync(pTaxista);
        }
    }
}
