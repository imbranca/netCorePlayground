using RestApiScratch.API.Models;

public class Instructor
{
  public int InstructorId {get;set;}
  public string? Initials {get;set;}
  public bool? HasBachellor{get;set;}
  public int UserId {get; set;}
  public User User {get; set;}
}