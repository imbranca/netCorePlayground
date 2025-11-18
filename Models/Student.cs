namespace RestApiScratch.API.Models;

public class Student : BaseEntity
{
    public int Id { get; set; }
    public int UserId {get; set;}
    public User User {get; set;}
    public string HomeAddress {get;set;}
    public bool? HasInsurance {get;set;}
    public bool? HasDependents {get;set;}

    public int? Status { get; set; } = 1; //1. enrolled 2. regular

    // public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}