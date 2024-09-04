using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SCHAPP.Models;
using System.Security.Claims;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Authorization;
using SCHAPP.Services;

namespace SCHAPP.Controllers
{
    public class AccountController : Controller
    {
        private readonly OracleDbContext _dbContext;
        private readonly string USUARIOS_PKG = "USUARIOS_PKG."; 
        private readonly CustomAuthenticationService _authService;


        public AccountController(IConfiguration configuration,CustomAuthenticationService authService)
        {
            _authService = authService;
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
        [Authorize(Roles = "CLIENTE")]
        public IActionResult Registrarse()
        {
            return View();
        }

        // POST: /Account/Login

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _authService.AuthenticateAsync(model.Username, model.Password);

            if (user != null && user.Roles.Count() > 0)
            {
                return Ok("Exito");
            }

            ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            return PartialView();

            /*if (IsValidUser(model.Username, model.Password))
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
