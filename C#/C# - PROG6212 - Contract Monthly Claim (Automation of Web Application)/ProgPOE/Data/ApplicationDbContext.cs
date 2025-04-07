using Microsoft.EntityFrameworkCore;
using ProgPOE.Models; // Namespace for your models

namespace ProgPOE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClaimModel> Claims { get; set; }
    }
}
