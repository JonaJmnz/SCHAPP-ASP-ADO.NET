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
         options.LoginPath = "/Home/Index"; // Ruta de inicio de sesi�n
         options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Duraci�n de la sesi�n
         options.SlidingExpiration = true; // Renovar la cookie
     });
builder.Services.AddControllersWithViews();

// Agregar configuraci�n para la cadena de conexi�n (opcional, ya est� en appsettings.json)
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();
// Configurar el middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error"); // P�gina de error en producci�n
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // Usar autenticaci�n
app.UseAuthorization(); // Usar autorizaci�n

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
