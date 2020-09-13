using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Guide.Models.Data
{
    public class GuideContext : IdentityDbContext<User>
    {
        public override DbSet<User> Users { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<TypeContent> TypeContents { get; set; }

        public virtual DbSet<TypeState> TypeStates { get; set; }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual DbSet<Glossary> Glossaries { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<TaskUser> TaskUsers { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<Step> Steps { get; set; }
        public virtual DbSet<IssueStep> IssueStep { get; set; }
        public virtual DbSet<Interpretation> Interpretations { get; set; }
        public virtual DbSet<DesiredResult> DesiredResults { get; set; }
        public virtual DbSet<DesiredResultIssue> DesiredResultIssue { get; set; }
        public virtual DbSet<DesiredResultStep> DesiredResultStep { get; set; }
        public virtual DbSet<BusinessProcess> BusinessProcesses { get; set; }
        public virtual DbSet<BusinessProcessIssue> BusinessProcessIssues { get; set; }
        public virtual DbSet<UserIssue> UserIssues { get; set; }
        public virtual DbSet<PositionIssue> PositionIssues { get; set; }
        public object UserRepository { get; set; }


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
            
            modelBuilder.Entity<Type>().HasData(new Type() {Id = 1, Name = "Книга", Active = true});
        }
        public GuideContext(DbContextOptions<GuideContext> options) : base(options)
        {
        }
        
    }
}

