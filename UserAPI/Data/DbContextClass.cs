using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI.Data
{
    public class DbContextClass : IdentityDbContext<ApplicationUser>
    {
        public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
        {

        }
        public DbContextClass()
        {

        }
        public DbSet<User> users { get; set; }
    }
}
