using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Motorista.Entidades.DeNegocio;
using Motorista.LogicaDeNegocio;
using System.Security.Claims;

namespace Motorista.UI.AppWebAspCore.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsuarioController : Controller
    {
        UsuarioBL usuarioBL = new UsuarioBL();
        RolBL rolBL = new RolBL();
        // GET: UsuarioController       
        public async Task<IActionResult> Index(Usuario pUsuario = null)
        {
            if (pUsuario == null)
                pUsuario = new Usuario();
            if (pUsuario.Top_aux == 0)
                pUsuario.Top_aux = 10;
            else if (pUsuario.Top_aux == -1)
                pUsuario.Top_aux = 0;
            var taskBuscar = usuarioBL.BuscarIncluirRolesAsync(pUsuario);
            var taskObtenerTodosRoles = rolBL.ObtenerTodosAsync();
            var usuarios = await taskBuscar;
            ViewBag.Top = pUsuario.Top_aux;
            ViewBag.Roles = await taskObtenerTodosRoles;
            return View(usuarios);
        }
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await usuarioBL.ObtenerPorIdAsync(new Usuario { Id = id });
            usuario.Rol = await rolBL.ObtenerPorIdAsync(new Rol { Id = usuario.IdRol });
            return View(usuario);
        }

        // GET: UsuarioController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = await rolBL.ObtenerTodosAsync();
            ViewBag.Error = "";
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario pUsuario)
        {
            try
            {
                int result = await usuarioBL.CrearAsync(pUsuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Roles = await rolBL.ObtenerTodosAsync();
                return View(pUsuario);
            }
        }
        // GET: UsuarioController/Edit/5
        public async Task<IActionResult> Edit(Usuario pUsuario)
        {
            var taskObtenerPorId = usuarioBL.ObtenerPorIdAsync(pUsuario);
            var taskObtenerTodosRoles = rolBL.ObtenerTodosAsync();
            var usuario = await taskObtenerPorId;
            ViewBag.Roles = await taskObtenerTodosRoles;
            ViewBag.Error = "";
            return View(usuario);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario pUsuario)
        {
            try
            {
                int result = await usuarioBL.ModificarAsync(pUsuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.Roles = await rolBL.ObtenerTodosAsync();
                return View(pUsuario);
            }
        }

        // GET: UsuarioController/Delete/5
        public async Task<IActionResult> Delete(Usuario pUsuario)
        {
            var usuario = await usuarioBL.ObtenerPorIdAsync(pUsuario);
            usuario.Rol = await rolBL.ObtenerPorIdAsync(new Rol { Id = usuario.IdRol });
            ViewBag.Error = "";
            return View(usuario);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Usuario pUsuario)
        {
            try
            {
                int result = await usuarioBL.EliminarAsync(pUsuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var usuario = await usuarioBL.ObtenerPorIdAsync(pUsuario);
                if (usuario == null)
                    usuario = new Usuario();
                if (usuario.Id > 0)
                    usuario.Rol = await rolBL.ObtenerPorIdAsync(new Rol { Id = usuario.IdRol });
                return View(usuario);
            }
        }

        // GET: UsuarioController/Login
        [AllowAnonymous]
        public async Task<IActionResult> Login(string ReturnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.Url = ReturnUrl;
            ViewBag.Error = "";
            return View();
        }

        // POST: UsuarioController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Usuario pUsuario, string pReturnUrl = null)
        {
            try
            {
                var usuario = await usuarioBL.LoginAsync(pUsuario);
                if (usuario != null && usuario.Id > 0 && pUsuario.Login == usuario.Login)
                {
                    usuario.Rol = await rolBL.ObtenerPorIdAsync(new Rol { Id = usuario.IdRol });
                    var claims = new[] { new Claim(ClaimTypes.Name, usuario.Login), new Claim(ClaimTypes.Role, usuario.Rol.Nombre) };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                }
                else
                    throw new Exception("Credenciales incorrectas");
                if (!string.IsNullOrWhiteSpace(pReturnUrl))
                    return Redirect(pReturnUrl);
                else
                    return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Url = pReturnUrl;
                ViewBag.Error = ex.Message;
                return View(new Usuario { Login = pUsuario.Login });
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> CerrarSesion(string ReturnUrl = null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Usuario");
        }
        // GET: UsuarioController/CambiarPassword
        public async Task<IActionResult> CambiarPassword()
        {

            var usuarios = await usuarioBL.BuscarAsync(new Usuario { Login = User.Identity.Name, Top_aux = 1 });
            var usuarioActual = usuarios.FirstOrDefault();
            ViewBag.Error = "";
            return View(usuarioActual);
        }

        // POST: UsuarioController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarPassword(Usuario pUsuario, string pPasswordAnt)
        {
            try
            {
                int result = await usuarioBL.CambiarPasswordAsync(pUsuario, pPasswordAnt);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Usuario");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                var usuarios = await usuarioBL.BuscarAsync(new Usuario { Login = User.Identity.Name, Top_aux = 1 });
                var usuarioActual = usuarios.FirstOrDefault();
                return View(usuarioActual);
            }
        }
        public async Task<int> TestCarga(string pKey, int pNum = 10)
        {
            int result = 0;
            var roles = await rolBL.BuscarAsync(new Rol { Top_aux = 10 });
            var dicRoles = new Dictionary<int, int>();
            int idRolPrimero = 0;
            for (var i = 0; i < roles.Count; i++)
            {
                if (i == 0)
                    idRolPrimero = roles[i].Id;
                dicRoles.Add(i, roles[i].Id);
            }
            int indexRol = 0;
            int indexStop = roles.Count - 1;
            for (var i = 0; i < pNum; i++)
            {
                var usuario = new Usuario();
                usuario.Nombre = string.Format("Nombre{0} {1}", i, pKey);
                usuario.Apellido = string.Format("Apellido{0} {1}", i, pKey);
                usuario.Login = string.Format("L{0}{1}", pKey, i);
                usuario.Password = string.Format("P{0}{1}", pKey, i);
                usuario.Estatus = (byte)Estatus_Usuario.ACTIVO;
                usuario.IdRol = dicRoles.ContainsKey(indexRol) ? dicRoles[indexRol] : idRolPrimero;
                result = +await usuarioBL.CrearAsync(usuario);
                indexRol++;
                if (indexRol >= indexStop)
                    indexRol = 0;
            }
            return result;
        }
    }
}
