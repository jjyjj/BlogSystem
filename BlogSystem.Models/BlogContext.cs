namespace BlogSystem.Models
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class BlogContext : DbContext
    {
        public BlogContext()
            : base("name=con")
        {
            Database.SetInitializer<BlogContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Fans> Fans { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleToCategory> ArticleToCategorys { get; set; }
        public DbSet<BlogCategory> BlogCategorys { get; set; }
        public DbSet<Comment> Comments { get; set; }


    }

}