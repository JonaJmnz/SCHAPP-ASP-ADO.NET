using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using SCHAPP.Models;
using System.Security.Claims;

namespace SCHAPP.Services
{
    public class CustomAuthenticationService
    {
        private readonly OracleDbContext _dbContext;
        //private readonly string USUARIOS_PKG = "USUARIOS_PKG."; 
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CustomAuthenticationService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor; 
            string connectionString = configuration.GetConnectionString("OracleDbContext");
            _dbContext = new OracleDbContext(connectionString);
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            string spName = "SP_USR_ROLS_CHECK";
            var roles = new List<string>();
            roles = _dbContext.UserRolesCheck("USUARIOS_PKG.", spName, username, password);
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, username)
                        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, "custom");
            var principal = new ClaimsPrincipal(identity);

            // Acceder a HttpContext a través de IHttpContextAccessor
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignInAsync("YourCookieScheme", principal);
            }
            return new User
            {
                User_name = username,
                Roles = roles
            };
        }

        private string HashPassword(string password)
        {
            // Implementación del hash de contraseña
            return password; // Ejemplo simplificado
        }
    }
}



