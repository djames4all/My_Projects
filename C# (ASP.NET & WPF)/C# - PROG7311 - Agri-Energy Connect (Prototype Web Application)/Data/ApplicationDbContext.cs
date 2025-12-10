using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prog7311_POE_Part2.Models;

namespace Prog7311_POE_Part2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Database Products Table
        public DbSet<Product> Products { get; set; }
    }
}
