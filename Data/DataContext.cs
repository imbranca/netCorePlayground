
using Microsoft.EntityFrameworkCore;
using RestApiScratch.API.Models;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
  public DbSet<Value> Values { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<Product> Products { get; set; }
  public DbSet<Course> Courses { get; set; }
  public DbSet<Student> Students { get; set; }
  public DbSet<Enrollment> Enrollments { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder){
    // Fluent API goes here
    modelBuilder.Entity<Enrollment>()
        .HasKey(e => new { e.StudentId, e.CourseId });
  }
}
