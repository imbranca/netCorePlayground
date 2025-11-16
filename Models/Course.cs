namespace RestApiScratch.API.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string CategoryId {get;set;}
    public Category Category {get;set;}
    // public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}