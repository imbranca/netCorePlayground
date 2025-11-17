namespace RestApiScratch.API.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int CategoryId {get;set;}
    public Category Category {get;set;}

    public int InstructorId { get; set; }
    public Instructor Instructor { get; set;}
    // public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}