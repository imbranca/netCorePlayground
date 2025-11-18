using Microsoft.AspNetCore.Identity;

namespace RestApiScratch.API.Models;


public class User : IdentityUser<int>
{
    // Add any custom fields you want here
    // (Identity already includes: UserName, PasswordHash, etc.)

    // Example custom fields:
       // NEW: A user has ONE Role
    public int RoleId { get; set; }

    // Navigation property
    public IdentityRole<int> Role { get; set; }

    // public Student? Student { get; set; }
    // public Instructor? Instructor { get; set; }
    public Profile? Profile { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public DateTime? DeletedAt { get; set; }
  //  public ICollection<IdentityUserRole<int>> UserRoles { get; set; }
}