using RestApiScratch.API.Models;

public class Category : BaseEntity
{
  public int CategoryId{get;set;}
  public string Name {get;set;}
  public ICollection<Course> Courses { get; set; }
}