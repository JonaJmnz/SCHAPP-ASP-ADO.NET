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
        private readonly CustomAuthenticationService _authService;

        public AccountController(IConfiguration configuration, CustomAuthenticationService authService)
        {
            _authService = authService;
            string connectionString = configuration.GetConnectionString("OracleDbContext");
            _dbContext = new OracleDbContext(connectionString);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return PartialView("Login");
        }
        //[Authorize(Roles = "CLIENTE")]
        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(RegistrarseViewModel model)
        {
            bool rol_insert_exito = false;
            bool usuario_insert_exito = false;
            string mensaje;
            // Aquí iría la lógica para guardar el empleado en la base de datos, similar al ejemplo anterior
            if (model.inpOtrServ != null)
            {
                rol_insert_exito = _dbContext.InsertRol("USUARIOS_PKG.", "SP_INSERT_ROL", model.inpOtrServ);
            }

            usuario_insert_exito = _dbContext.InsertUser("USUARIOS_PKG.", "SP_INSERT_USER", model);
            if (usuario_insert_exito && rol_insert_exito)
            {
                mensaje = "Exit";
            }
            else
            {
                mensaje = "Fail";
            }
            // Devolver el mensaje como JSON
            return Json(new { mensaje });
        }
        [HttpPost]
        public async Task<IActionResult> ExisteUsuario(string nombreUsuario)
        {
            bool existe = true;
            if (nombreUsuario != "")
            {
                existe = _dbContext.ExistUserName("USUARIOS_PKG.", "SP_VALIDA_N_USUARIO", nombreUsuario);
                if (existe)
                {
                    ModelState.AddModelError("inpNombreUsu", "El usuario ya existe en la base de datos.");
                }
            }
            return Ok(existe);
        }
        [HttpPost]
        public async Task<IActionResult> traerRoles()
        {
            List<string> roles = new List<string>();
            roles = _dbContext.GetRoles("USUARIOS_PKG.", "SP_GET_ROLES");
            
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> ExisteEmail(string email)
        {
            bool existe = true;
            if (email != null)
            {
                existe = _dbContext.ExistUserName("USUARIOS_PKG.", "SP_VALIDA_EMAIL", email);
                if (existe)
                {
                    ModelState.AddModelError("inpEmail","El email ya está resitrado");
                }
            }
            return Ok(existe);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _authService.AuthenticateAsync(model.Username, model.Password);

            if (user != null && user.Roles.Count() > 0)
            {
                return Ok("Exito");
            }
            ModelState.AddModelError("Password", "Nombre de usuario o contraseña incorrectos.");
            return PartialView();
        }
        public async Task<IActionResult> Logout()
        {
            // Eliminar la autenticación (claims-based)
            await HttpContext.SignOutAsync();

            // Redirigir al usuario a la página de inicio o login
            return RedirectToAction("Index", "Home");// Cerrar sesión
        }
    }
}
