using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
     {
         options.LoginPath = "/Home/Index"; // Ruta de inicio de sesión
         options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Duración de la sesión
         options.SlidingExpiration = true; // Renovar la cookie
     });
builder.Services.AddControllersWithViews();

// Agregar configuración para la cadena de conexión (opcional, ya está en appsettings.json)
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();
// Configurar el middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Página de error en producción
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Usar autenticación
app.UseAuthorization(); // Usar autorización

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
