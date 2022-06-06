using FreeLancing.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FreeLancing.Models.VMs;

namespace FreeLancing.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<CustomTag> CustomTags { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<FreeLancing.Models.VMs.PostNewJobVM> PostNewJobVM { get; set; }
    }
}
