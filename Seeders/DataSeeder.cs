
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RestApiScratch.API.Models;
using Bogus;
namespace MyApp.Namespace;

public class DataSeeder : ISeeder
{
  public async Task SeedAsync(AppDbContext context, IServiceProvider services)
  {
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

    // ---- Seed Roles ----
    var roles = new[] { "admin", "user" };

    foreach (var role in roles)
    {
      if (!await roleManager.RoleExistsAsync(role))
      {
        await roleManager.CreateAsync(new IdentityRole<int>(role));
      }
    }
    var adminUser = "admin";
    var adminEmail = "admin@example.com";
    var adminPassword = "Admin123!";

    if (await userManager.FindByNameAsync(adminUser) == null)
    {
      var user = new User
      {
        UserName = adminUser,
        Email = adminEmail,
        EmailConfirmed = true,
        CreatedAt = DateTime.UtcNow
      };

      var result = await userManager.CreateAsync(user, adminPassword);

      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(user, "Admin");
      }
    }
 
    var simpleUser = "lusho";
    var simpleEmail = "lusho@example.com";
    var simplePassword =  "Admin123!";

    if (await userManager.FindByNameAsync(simpleUser) == null)
    {
      var user = new User
      {
        UserName = simpleUser,
        Email = simpleEmail,
        EmailConfirmed = true,
        CreatedAt = DateTime.UtcNow
      };

      var result = await userManager.CreateAsync(user, simplePassword);

      if (result.Succeeded)
      {
        await userManager.AddToRoleAsync(user, "user");
      }
    }
    await context.SaveChangesAsync();

  }
}