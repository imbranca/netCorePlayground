using Bogus;
using Microsoft.AspNetCore.Identity;
using MyApp.Namespace;
using RestApiScratch.API.Models;

public class CoursesSeeder : ISeeder
{

  public async Task SeedAsync(AppDbContext context, IServiceProvider services)
  {
    // // var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    // var db = services.GetRequiredService<AppDbContext>();
    // var categoriesList = new[] {
    //     "Science",
    //     "Mathematics",
    //     "Languages",
    //     "Programming",
    //     "Business",
    //     "History"
    // };

    // var generatedCategories = categoriesList
    //     .Select(name => new Category
    //     {
    //         Name = name,
    //         Courses = new List<Course>()
    //     })
    //     .ToList();
    // context.Categories.AddRange(generatedCategories);
    // await context.SaveChangesAsync();


    // var generatedCategoriesContext = context.Categories.ToList();
    // var instructors = context.Instructors.ToList();

    // var courseFaker = new Faker<Course>()
    //   .RuleFor(c => c.Id, f => 0)  // EF will set it
    //   .RuleFor(c => c.Title, f => f.Commerce.ProductName())
    //   .RuleFor(c => c.Description, f => f.Lorem.Paragraph())
    //   .RuleFor(c => c.Price, f => f.Random.Decimal(19, 199))

    //   // assign a random category
    //   .RuleFor(c => c.CategoryId, f => f.PickRandom(generatedCategoriesContext).CategoryId)
    //   .RuleFor(c => c.Category, (f, c) =>
    //       generatedCategoriesContext.First(x => x.CategoryId == c.CategoryId))

    //   // assign a random instructor
    //   .RuleFor(c => c.InstructorId, f => f.PickRandom(instructors).InstructorId)
    //   .RuleFor(c => c.Instructor, (f, c) =>
    //       instructors.First(x => x.InstructorId == c.InstructorId));

    // var generatedFakeCourses = courseFaker.Generate(20);
    // context.Courses.AddRange(generatedFakeCourses);
    // await context.SaveChangesAsync();

    var students = context.Students.ToList();
    var courses = context.Courses.ToList();

    var enrollmentFaker = new Faker<Enrollment>()
    .RuleFor(e => e.StudentId, f => f.PickRandom(students).Id)
    .RuleFor(e => e.CourseId, f => f.PickRandom(courses).Id)
    .RuleFor(e => e.EnrollmentDate, f => f.Date.Past(1));
    var generatedEnrollment = enrollmentFaker.Generate(5);
    context.AddRange(generatedEnrollment);
    await context.SaveChangesAsync();

  }
}