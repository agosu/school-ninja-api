using Microsoft.EntityFrameworkCore;
using SchoolNinjaAPI.Models;

namespace SchoolNinjaAPI.Data
{
    public class SchoolNinjaAPIContext : DbContext
    {
        public SchoolNinjaAPIContext (DbContextOptions<SchoolNinjaAPIContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
