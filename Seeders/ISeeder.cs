
namespace MyApp.Namespace;

public interface ISeeder
{
    Task SeedAsync(AppDbContext context, IServiceProvider services);
}