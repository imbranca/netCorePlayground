using Bogus;
using Microsoft.AspNetCore.Identity;
using MyApp.Namespace;
using RestApiScratch.API.Models;

public class UsersSeeder : ISeeder{

  public async Task SeedAsync(AppDbContext context, IServiceProvider services)
  {
    // var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var db = services.GetRequiredService<AppDbContext>();
    var roles = db.Roles.ToList(); // IdentityRole<int>

    // var userIds = 3;
    var passwordHasher = new PasswordHasher<User>();

    var env = services.GetRequiredService<IHostEnvironment>();
    // var roles = roleManager.Roles.ToList();
    var userFaker = new Faker<User>()
        // .RuleFor(u => u.Id, f => userIds++)
        .RuleFor(u => u.UserName, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.NormalizedUserName, (f, u) => u.UserName.ToUpper())
        .RuleFor(u => u.NormalizedEmail, (f, u) => u.Email.ToUpper())
        .RuleFor(u => u.SecurityStamp, f => Guid.NewGuid().ToString())
        .RuleFor(u => u.ConcurrencyStamp, f => Guid.NewGuid().ToString())
        .RuleFor(u => u.RoleId, f => f.PickRandom(roles).Id)
        .FinishWith((f, user) =>
        {
            // Every user gets password: P@ss123
            user.PasswordHash = passwordHasher.HashPassword(user, "P@ss123");
        });

    context.Users.AddRange(userFaker.Generate(10));
    await context.SaveChangesAsync();

    //SEED Students
    // var studentsFaker = new Faker<Student>();

    //SEED Instructors
  }

}