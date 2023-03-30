using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Motorista.AccesoADatos;
using Motorista.Entidades.DeNegocio;
using Motorista.LogicaDeNegocio;

namespace Motorista.UI.AppWebAspCore.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class TaxistaController : Controller
    {
        TaxistaBL taxistaBL = new TaxistaBL();
        // GET: TaxistaController
        public async Task<ActionResult> Index(Taxista pTaxista = null)
        {
            if (pTaxista == null)
                pTaxista = new Taxista();
            if (pTaxista.Top_aux == 0)
                pTaxista.Top_aux = 10;
            else if (pTaxista.Top_aux == -1)
                pTaxista.Top_aux = 0;
            var taxistas = await taxistaBL.BuscarAsync(pTaxista);
            ViewBag.Top = pTaxista.Top_aux;
            return View(taxistas);
        }

        // GET: TaxistaController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var taxista = await taxistaBL.ObtenerPorIdAsync(new Taxista { Id = id });
            return View(taxista);
        }

        // GET: TaxistaController/Create
        public ActionResult Create()
        {
            ViewBag.Error = "";
            return View();
        }

        // POST: TaxistaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Taxista pTaxista)
        {
            try
            {
                int result = await taxistaBL.CrearAsync(pTaxista);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(pTaxista);
            }
        }

        // GET: TaxistaController/Edit/5
        public async Task<ActionResult> Edit(Taxista pTaxista)
        {
            var taxista = await taxistaBL.ObtenerPorIdAsync(pTaxista);
            ViewBag.Error = "";
            return View(taxista);
        }

        // POST: TaxistaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Taxista pTaxista)
        {
            try
            {
                int result = await taxistaBL.ModificarAsync(pTaxista);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;    
                return View(pTaxista);
            }
        }

        // GET: TaxistaController/Delete/5
        public async Task<ActionResult> Delete(Taxista pTaxista)
        {
            var taxista = await taxistaBL.ObtenerPorIdAsync(pTaxista);
            return View(taxista);
        }

        // POST: TaxistaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Taxista pTaxista)
        {
            try
            {
                int result = await taxistaBL.EliminarAsync(pTaxista);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(pTaxista);
            }
        }
        public async Task<int> TestCarga(string pKey, int pNum = 10)
        {
            int result = 0;
            for (var i = 0; i < pNum; i++)
            {
                var taxista = new Taxista();
                taxista.Nombre = string.Format("Nombre{0} {1}", i, pKey);
                taxista.Apellido = string.Format("Apellido{0} {1}", i, pKey);
                taxista.Placa = string.Format("Placa{0} {1}", i, pKey);
                taxista.Color = (byte)Color.Azul;
                taxista.Marca = (byte)Marca.Toyota;
                taxista.Estatus = (byte)Estatus.ACTIVO;
                taxista.Nacionalidad = (byte)Nacionalidad.Salvadoreño;
                taxista.Estado = (byte)Estado.Nuevo;
                result = await taxistaBL.CrearAsync(taxista);
            }
            return result;
        }
    }
}
