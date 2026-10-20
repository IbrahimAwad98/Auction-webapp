using Microsoft.AspNetCore.Identity;

namespace AuctionApp.Data.Identity;

// Creates the Admin role and one admin account when the app starts.

// The password is read from configuration (user-secrets), never from the code.
public static class IdentitySeeder
{
    public const string AdminRole = "Admin";
    
   
    public static async Task SeedAdminAsync(IServiceProvider services, IConfiguration config)
    {
        using IServiceScope scope = services.CreateScope();
        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        
        if (!await roleManager.RoleExistsAsync(AdminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(AdminRole));
        }
        
        string email = config["Admin:Email"] ?? "admin@kth.se";
        string? password = config["Admin:Password"];
        if (string.IsNullOrEmpty(password))
        {
            return;   
        }
        
        IdentityUser? admin = await userManager.FindByEmailAsync(email);
        if (admin == null)
        {
            admin = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    "Could not create admin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        
        if (!await userManager.IsInRoleAsync(admin, AdminRole))
        {
            await userManager.AddToRoleAsync(admin, AdminRole);
        }
    }
}