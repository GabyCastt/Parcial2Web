using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GestionProyectoFINAL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<GestionProyectosContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuración de Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<GestionProyectosContext>()
.AddDefaultUI() 
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Configuración de políticas de autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministrador", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("RequireGestor", policy => policy.RequireRole("Gestor"));
});

var app = builder.Build();

// Inicialización de datos
using (var scope = app.Services.CreateScope())
{
    await InitialSetup.Initialize(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configuración de endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();