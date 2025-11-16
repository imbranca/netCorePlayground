
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
    var roles = new[] { "admin", "instructor", "user" };

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
      var role = await roleManager.FindByNameAsync("admin");
      var user = new User
      {
        UserName = adminUser,
        Email = adminEmail,
        EmailConfirmed = true,
        CreatedAt = DateTime.UtcNow,
        //Admin
        RoleId = role.Id
      };

      var result = await userManager.CreateAsync(user, adminPassword);
    }


    var instructorUser = "instructor";
    var instructorEmail = "instructor@example.com";
    var instructorPassword = "Instructor123!";

    if (await userManager.FindByNameAsync(adminUser) == null)
    {
      var role = await roleManager.FindByNameAsync("instructor");
      var user = new User
      {
        UserName = instructorUser,
        Email = instructorEmail,
        EmailConfirmed = true,
        CreatedAt = DateTime.UtcNow,
        //Admin
        RoleId = role.Id
      };

      var result = await userManager.CreateAsync(user, instructorPassword);
    }


    var simpleUser = "lusho";
    var simpleEmail = "lusho@example.com";
    var simplePassword =  "Admin123!";
    var roleUser = await roleManager.FindByNameAsync("user");

    if (await userManager.FindByNameAsync(simpleUser) == null)
    {
      var user = new User
      {
        UserName = simpleUser,
        Email = simpleEmail,
        EmailConfirmed = true,
        CreatedAt = DateTime.UtcNow,
        RoleId = roleUser.Id
      };

      var result = await userManager.CreateAsync(user, simplePassword);
    }
    await context.SaveChangesAsync();
    //Create Student

  }
}