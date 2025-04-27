namespace Camp_Rating_System.Data
{

    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public static class DatabaseSeedData
    {
        public static async void Initialize(WebApplication app)
        {

            using (var serviceScope = app.Services.CreateScope())
            {
                using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ProjectDbContext>())
                {
                    dbContext.Database.Migrate();
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    // Seed roles
                    await SeedRolesAsync(roleManager);

                    // Seed a default admin user
                    await SeedAdminUserAsync(userManager);
                }
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager) 
        {
            var roleNames = new[] { "Admin", "User"};

            foreach (var roleName in roleNames)
            {
                try
                {
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"Error: {error.Description}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Role {roleName} created successfully.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Role {roleName} already exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating role {roleName}: {ex.Message}");
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByEmailAsync("admin@admin.com");
            if (user == null)
            {
                user = new ApplicationUser { 
                    FirstName = "admin", 
                    LastName = "",
                    UserName = "admin@admin.com", 
                    Email = "admin@admin.com"
                };
                var createUser = await userManager.CreateAsync(user, "Admin123!");

                if (createUser.Succeeded)
                {
                    await userManager.UpdateAsync(user);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
