using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Oto_Api.Domain.Entities;

public class DataSeed
{
   public static async Task DataSeedAdmin(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string adminEmail = "admin@example.com";
        string adminPassword = "Admin@123";

        // Tạo role nếu chưa có
        if (!await roleManager.RoleExistsAsync("admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("admin"));
        }

        // Tạo admin nếu chưa tồn tại
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var user = new User
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            var result = await userManager.CreateAsync(user, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}

