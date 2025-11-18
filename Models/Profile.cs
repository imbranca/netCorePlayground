
using RestApiScratch.API.Models;

public class Profile : BaseEntity
{
  public int ProfileId { get; set; }

  public int UserId {get; set;}
  public User User {get; set;}

  public string Bio {get; set;}
  public string Avatar {get; set;}
}