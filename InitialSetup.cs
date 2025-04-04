using GestionProyectoFINAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class InitialSetup
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Roles a crear
            string[] roleNames = { "Administrador", "Gestor" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Crear usuario admin por defecto
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(user, "Admin123!");
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrador");
                }
            }
            // Crear usuario gestor
            var gestorUser = await userManager.FindByEmailAsync("gestor@example.com");
            if (gestorUser == null)
            {
                var gestor = new ApplicationUser
                {
                    UserName = "gestor@example.com",
                    Email = "gestor@example.com",
                    EmailConfirmed = true
                };
                var createGestor = await userManager.CreateAsync(gestor, "Gestor123!");
                if (createGestor.Succeeded)
                {
                    await userManager.AddToRoleAsync(gestor, "Gestor");
                }
            }
        }
    }
}