using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guide.Models.Data
{
    public class GuideContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Glossary> Glossaries { get; set; }
        public DbSet<Position> Positions { get; set; }

        public GuideContext(DbContextOptions<GuideContext> options) : base(options) {}
    }
}