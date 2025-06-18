using CleanArchitectureProject.Infrastructure.Data;
using CleanArchitectureProject.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Admin
        string adminEmail = "admin@localhost";
        string adminPass = "Admin1!";

        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var admin = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, adminPass);
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        // User
        string userEmail = "user@localhost";
        string userPass = "User1!";

        if (await userManager.FindByEmailAsync(userEmail) is null)
        {
            var user = new IdentityUser
            {
                UserName = userEmail,
                Email = userEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, userPass);
            await userManager.AddToRoleAsync(user, "User");
        }
    }
}
