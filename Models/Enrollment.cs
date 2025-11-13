namespace RestApiScratch.API.Models;

public class Enrollment
{
  public int StudentId { get; set; }
  public Student Student { get; set; }

  public int CourseId { get; set; }
  public Course Course { get; set; }

  // Extra data on the relationship
  public DateTime EnrollmentDate { get; set; }
}