
using System.Security.Claims;
using Humanizer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using RestApiScratch.API.Models;

namespace MyApp.Namespace
{

  [Route("api/[controller]")]
  [ApiController]
  public class StudentController : ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    private readonly TokenService _tokenService;

    public StudentController(AppDbContext productsContext,
     TokenService tokenService,
     UserManager<User> userManager,
     SignInManager<User> signInManager)
    {
      _context = productsContext;
      _userManager = userManager;
      _signInManager = signInManager;
      _tokenService = tokenService;
    }

    [HttpGet("blocking")]
    public IActionResult B()
    {
        Thread.Sleep(TimeSpan.FromMinutes(3)); // blocking!
        return Ok("Hello from endpoint B (after 3 minutes)");
    }

  [HttpPost ("enroll")]
  public async Task<IActionResult> EnrollCourse([FromBody] StudentCourseDTO studentCourseDto)
    {
      var student = _context.Students.First(x=> x.Id == studentCourseDto.StudentId);
      var course = _context.Courses.First(x=> x.Id == studentCourseDto.CourseId);
      var enroll = new Enrollment{
        CourseId = course.Id,
        StudentId = student.Id,
        EnrollmentDate = DateTime.UtcNow
        };
      _context.Enrollments.Add(enroll);
      await _context.SaveChangesAsync();

      return Ok(new
      {
          message = "Enrollment successful",
          studentId = enroll.StudentId,
          courseId = enroll.CourseId,
          date = enroll.EnrollmentDate
      });
    }
  }
}
