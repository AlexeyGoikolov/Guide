using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guide.Models.Data
{
    public class GuideContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }

        public GuideContext(DbContextOptions<GuideContext> options) : base(options) {}
    }
}