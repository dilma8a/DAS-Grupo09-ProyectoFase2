using DAS_Grupo09_ProyectoFase2.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// =========================
// MVC
// =========================
builder.Services.AddControllersWithViews();

// =========================
// SESSION
// =========================
builder.Services.AddDistributedMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // tiempo de inactividad
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// =========================
// NECESARIO PARA USAR HttpContext EN EL LAYOUT (@inject IHttpContextAccessor)
// =========================
builder.Services.AddHttpContextAccessor();

// =========================
// HTTP CLIENTS PARA LA API
// =========================
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "https://localhost:7218/api/";

builder.Services.AddHttpClient<ILoginService, LoginService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<IClienteService, ClienteService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<IPaqueteService, PaqueteService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<IEnvioService, EnvioService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<IReclamoService, ReclamoService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<IReporteService, ReporteService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

var app = builder.Build();

// =========================
// PIPELINE
// =========================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// MUY IMPORTANTE: antes de Authorization
app.UseSession();

app.UseAuthorization();

// Ruta por defecto: Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
