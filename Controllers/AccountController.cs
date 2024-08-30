using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCHAPP.Models;
using System.Security.Claims;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Authorization;

namespace SCHAPP.Controllers
{
    public class AccountController : Controller
    {
        private readonly OracleDbContext _dbContext;
        private readonly string USUARIOS_PKG = "USUARIOS_PKG.";

        public AccountController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("OracleDbContext");
            _dbContext = new OracleDbContext(connectionString);
        }

        [HttpGet]
        public IActionResult ObtenerDatos()
        {
            // Consulta de ejemplo
            string consulta = "SELECT * FROM usuarios";
            var datos = _dbContext.EjecutarConsulta(consulta);

            return View(datos); // Retorna los datos a la vista
        }
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return PartialView("Login");
        }
        [Authorize]
        public IActionResult Registrarse()
        {
            return View();
        }

        // POST: /Account/Login

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (IsValidUser(model.Username, model.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                //return RedirectToAction("Index", "Home"); // Redirigir a la página principal
                return Ok("Exito"); 
            }

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
            return PartialView();
            /*if (ModelState.IsValid)
            {
                // Suponiendo que tienes una lógica para validar el usuario
                //var isValidUser = (model.Username == "admin" && model.Password == "admin");

                // Consulta de ejemplo
                //string consulta = "SELECT * FROM usuarios";
                //var datos = _dbContext.EjecutarConsulta(consulta);

                if(IsValidUser(model.Username, model.Password))
                {
                    return Ok("Exito");
                }

                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
            }

            return PartialView();*/
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Cerrar sesión
            return RedirectToAction("Home","Index");
        }
        private bool IsValidUser(string username, string password)
        {
            //Ejecucion de sp - consulta el usuario en la db
            string spName = "SP_OBTENERUSUARIO";
            int DBLogin = _dbContext.ExecuteStoredProcedure(USUARIOS_PKG, spName, username, password);

            if (DBLogin == 1)
            {
                return true;
            }
            return false;
        }
    }
}
