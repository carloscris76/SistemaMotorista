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
    public class ClienteController : Controller
    {
        ClienteBL clienteBL = new ClienteBL();
        TaxistaBL taxistaBL = new TaxistaBL();
        // GET: ClienteController
        public async Task<ActionResult> Index(Cliente pCliente = null)
        {
            if (pCliente == null)
                pCliente = new Cliente();
            if (pCliente.Top_aux == 0)
                pCliente.Top_aux = 10;
            else if (pCliente.Top_aux == -1)
                pCliente.Top_aux = 0;
            var taskBuscar = clienteBL.BuscarIncluirTaxistaAsync(pCliente);
            var taskObtenerTodosTaxistas = clienteBL.BuscarIncluirTaxistaAsync(pCliente);
            var clientes = await taskBuscar;
            ViewBag.Top = pCliente.Top_aux;
            ViewBag.Taxistas = await taskObtenerTodosTaxistas;
            return View(clientes);
        }

        // GET: ClienteController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var cliente = await clienteBL.ObtenerPorIdAsync(new Cliente { Id = id });
            cliente.Taxista = await taxistaBL.ObtenerPorIdAsync(new Taxista { Id = cliente.IdTaxista });
            return View(cliente);
        }

        // GET: ClienteController/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Taxistas = await taxistaBL.ObtenerTodosAsync();
            ViewBag.Error = "";
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente pCliente)
        {
            try
            {
                int result = await clienteBL.CrearAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Taxistas = await taxistaBL.ObtenerTodosAsync();
                return View(pCliente);
            }
        }

        // GET: ClienteController/Edit/5
        public async Task<ActionResult> Edit(Cliente pCliente)
        {
            var taskObtenerPorId = clienteBL.ObtenerPorIdAsync(pCliente);
            var taskObtenerTodosTaxistas = taxistaBL.ObtenerTodosAsync();
            var cliente = await taskObtenerPorId;
            ViewBag.Taxistas = await taskObtenerTodosTaxistas;
            ViewBag.Error = "";
            return View(cliente);
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Cliente pCliente)
        {
            try
            {
                int result = await clienteBL.ModificarAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Taxistas = await taxistaBL.ObtenerTodosAsync();
                return View(pCliente);
            }
        }

        // GET: ClienteController/Delete/5
        public async Task<ActionResult> Delete(Cliente pCliente)
        {
            var cliente = await clienteBL.ObtenerPorIdAsync(pCliente);
            cliente.Taxista = await taxistaBL.ObtenerPorIdAsync(new Taxista { Id = cliente.IdTaxista });
            ViewBag.Error = "";
            return View(cliente);
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, Cliente pCliente)
        {
            try
            {
                int result = await clienteBL.EliminarAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var cliente = await clienteBL.ObtenerPorIdAsync(pCliente);
                if (cliente == null)
                    cliente = new Cliente();
                if (cliente.Id > 0)
                    cliente.Taxista = await taxistaBL.ObtenerPorIdAsync(new Taxista { Id = cliente.IdTaxista });
                return View(cliente);
            }
        }
        public async Task<int> TestCarga(string pKey, int pNum = 10)
        {
            int result = 0;
            var taxistas = await taxistaBL.BuscarAsync(new Taxista { Top_aux = 10 });
            var dicTaxistas = new Dictionary<int, int>();
            int idTaxistaPrimero = 0;
            for (var i = 0; i < taxistas.Count; i++)
            {
                if (i == 0)
                    idTaxistaPrimero = taxistas[i].Id;
                dicTaxistas.Add(i, taxistas[i].Id);
            }
            int indexTaxista = 0;
            int indexStop = taxistas.Count - 1;
            for (var i = 0; i < pNum; i++)
            {
                var cliente = new Cliente();
                cliente.Nombre = string.Format("Nombre{0} {1}", i, pKey);
                cliente.Apellido = string.Format("Apellido{0} {1}", i, pKey);
                cliente.TipoCliente = (byte)TipoCliente.Empresarial;
                cliente.IdTaxista = dicTaxistas.ContainsKey(indexTaxista) ? dicTaxistas[indexTaxista] : idTaxistaPrimero;
                result = +await clienteBL.CrearAsync(cliente);
                indexTaxista++;
                if (indexTaxista >= indexStop)
                    indexTaxista = 0;
            }
            return result;
        }
    }
}
