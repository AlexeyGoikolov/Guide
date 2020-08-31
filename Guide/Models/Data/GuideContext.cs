using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guide.Models.Data
{
    public class GuideContext : IdentityDbContext<User>
    {
        public override DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<TypeContent> TypeContents { get; set; }

        public DbSet<TypeState> TypeStates { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Glossary> Glossaries { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<IssueStep> IssueStep { get; set; }
        public DbSet<Interpretation> Interpretations { get; set; }
        public DbSet<DesiredResult> DesiredResults { get; set; }
        public DbSet<DesiredResultIssue> DesiredResultIssue { get; set; }
        public DbSet<BusinessProcess> BusinessProcesses { get; set; }
        public DbSet<BusinessProcessIssue> BusinessProcessIssues { get; set; }
        
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IssueStep>()
                .HasKey(t => new {t.Id});

            modelBuilder.Entity<IssueStep>()
                .HasOne(isc => isc.Issue)
                .WithMany(i => i.IssueSteps)
                .HasForeignKey(isc => isc.IssueId);

            modelBuilder.Entity<IssueStep>()
                .HasOne(isc => isc.Step)
                .WithMany(s => s.IssueSteps)
                .HasForeignKey(isc => isc.StepId);
        }
        public GuideContext(DbContextOptions<GuideContext> options) : base(options)
        {
        }

        
    }
}

