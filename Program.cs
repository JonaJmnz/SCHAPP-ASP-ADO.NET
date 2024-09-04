using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SCHAPP.Authorization;
using SCHAPP.Controllers;
using SCHAPP.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de autenticaci�n y autorizaci�n
builder.Services.AddAuthentication("YourCookieScheme")
    .AddCookie("YourCookieScheme", options =>
    {
        options.LoginPath = "/Account/Login";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("Admin", "Manager");
    });
});

// Registrar servicios personalizados
builder.Services.AddScoped<CustomAuthenticationService>();
builder.Services.AddSingleton<IAuthorizationHandler, RoleAuthorizationHandler>();
builder.Services.AddHttpContextAccessor();

// Configurar servicios MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
























/*
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

app.Run();*/
