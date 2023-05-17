using Microsoft.AspNetCore.Identity;

namespace Karmaplusplus.Models
{
  public class ApplicationUser : IdentityUser
  {
    public string FirstName { get; set; }
    
  }
}