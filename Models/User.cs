using Microsoft.AspNetCore.Identity;

namespace RestApiScratch.API.Models;


public class User : IdentityUser<int>
{
    // Add any custom fields you want here
    // (Identity already includes: UserName, PasswordHash, etc.)

    // Example custom fields:
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   public ICollection<IdentityUserRole<int>> UserRoles { get; set; }

}