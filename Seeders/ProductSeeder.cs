using Bogus;
using RestApiScratch.API.Models;

namespace MyApp.Namespace;

public class ProductSeeder : ISeeder
{
  public async Task SeedAsync(AppDbContext context, IServiceProvider services)
  {
     var env = services.GetRequiredService<IHostEnvironment>();
        // if (!env.IsDevelopment() || context.Users.Any()) return;

        var faker = new Faker<Product>()
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.Price, f => f.Random.Decimal(5, 500));
  
      context.Products.AddRange(faker.Generate(50));
      await context.SaveChangesAsync();
  }
}