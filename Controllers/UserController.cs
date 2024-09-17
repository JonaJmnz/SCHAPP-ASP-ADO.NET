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
    [Authorize] // Asegúrate de que este controlador solo pueda ser accedido por usuarios autenticados
    [Route("api/user/claims")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUserClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }
    }
}
