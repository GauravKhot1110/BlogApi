using BlogAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BlogAPI.Helper
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLogin>()
                .HasKey(c => new { c.UserID, c.FirstName, c.LastName, c.Email, c.MobileNumber, c.Geneder, c.Password, c.IsActive });

            modelBuilder.Entity<BlogMaster>()
                .HasKey(c => new { c.UserID, c.Title, c.Description,c.Filename, c.CountViews, c.CountLikes, c.CountUnLikes, c.CountComments, c.CreateBy ,c.IsActive });
           
            modelBuilder.Entity<ActionDetails>()
                .HasKey(c => new { c.ActionUserID, c.ActionBy, c.ActionFor, c.IsLikeUnlikeComment, c.Comment,  c.CreateDate,c.IsActive });
        }

        public DbSet<UserLogin> UserLogin
        {
            get;
            set;
        }
        public DbSet<BlogMaster> BlogMaster
        {
            get;
            set;
        }

        public DbSet<ActionDetails> ActionDetails
        {
            get;
            set;
        }
    }
}
