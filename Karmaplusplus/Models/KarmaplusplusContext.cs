using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Karmaplusplus.Models
{
  public class KarmaplusplusContext: IdentityDbContext<ApplicationUser>
  {
    public DbSet<Service> Services { get; set; }

    public KarmaplusplusContext(DbContextOptions options) : base(options) { }
  }
}