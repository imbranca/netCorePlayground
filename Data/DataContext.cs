
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestApiScratch.API.Models;

public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
      public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Value> Values { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder){

      base.OnModelCreating(modelBuilder);

      foreach (var entityType in modelBuilder.Model.GetEntityTypes())
      {
          if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
          {
              var parameter = Expression.Parameter(entityType.ClrType, "e");
              var deletedAtProperty = Expression.Property(parameter, nameof(BaseEntity.DeletedAt));
              var nullConstant = Expression.Constant(null, typeof(DateTime?));
              var body = Expression.Equal(deletedAtProperty, nullConstant);

              var lambda = Expression.Lambda(body, parameter);

              modelBuilder.Entity(entityType.ClrType)
                  .HasQueryFilter(lambda);
          }
      }
    // Fluent API goes here
    modelBuilder.Entity<Enrollment>()
          .HasKey(e => new { e.StudentId, e.CourseId });

      modelBuilder.Entity<Product>()
      .Property(x=> x.Price).HasPrecision(18,2);

      modelBuilder.Entity<IdentityUserRole<int>>()
      .HasKey(x => new { x.UserId, x.RoleId });

    modelBuilder.Entity<User>()
        .HasOne(u => u.Profile)
        .WithOne(p => p.User)
        .HasForeignKey<Profile>(p => p.UserId)
        .IsRequired(false);

      // Configure ONE role per user
      modelBuilder.Entity<User>()
          .HasOne(u => u.Role)
          .WithMany()
          .HasForeignKey(u => u.RoleId)
          .OnDelete(DeleteBehavior.Restrict);

      // // Optional 1–1 User → Student
      // modelBuilder.Entity<User>()
      //     .HasOne(u => u.Student)
      //     .WithOne(s => s.User)
      //     .HasForeignKey<Student>(s => s.UserId)
      //     .IsRequired(false);

      // // Optional 1–1 User → Instructor
      // modelBuilder.Entity<User>()
      //     .HasOne(u => u.Instructor)
      //     .WithOne(i => i.User)
      //     .HasForeignKey<Instructor>(i => i.UserId)
      //     .IsRequired(false);

      // Optional 1–1 User → Profile
      modelBuilder.Entity<User>()
          .HasOne(u => u.Profile)
          .WithOne(p => p.User)
          .HasForeignKey<Profile>(p => p.UserId)
          .IsRequired(false);

      modelBuilder.Entity<Course>()
            .HasOne(u=> u.Category)
            .WithMany(p => p.Courses);
      
      modelBuilder.Entity<Course>()
            .HasOne(u => u.Instructor)
            .WithMany(p => p.Courses);

      modelBuilder.Entity<Student>()
        .HasOne(s => s.User)
        .WithMany()
        .HasForeignKey(s => s.UserId)
        .OnDelete(DeleteBehavior.NoAction);

      modelBuilder.Entity<Instructor>()
      .HasOne(i => i.User)
      .WithMany()
      .HasForeignKey(i => i.UserId)
      .OnDelete(DeleteBehavior.NoAction);

    modelBuilder.Entity<Profile>()
        .HasOne(p => p.User)
        .WithOne()
        .HasForeignKey<Profile>(p => p.UserId)
        .OnDelete(DeleteBehavior.NoAction);

          modelBuilder.Entity<User>().HasOne(u => u.Profile);

          // modelBuilder.Entity<User>().HasOne(u => u.Student);
          // modelBuilder.Entity<User>().HasOne(u => u.Instructor);

          // OPTIONAL: Remove the join table from EF
          modelBuilder.Ignore<IdentityUserRole<int>>();
        }

    public override int SaveChanges()
    {
      foreach (var entry in ChangeTracker.Entries())
      {
        if (entry.Entity is BaseEntity auditable)
        {
          if (entry.State == EntityState.Added)
            auditable.CreatedAt = DateTime.UtcNow;

          auditable.UpdatedAt = DateTime.UtcNow;
        }
      }

      return base.SaveChanges();
    }
}
