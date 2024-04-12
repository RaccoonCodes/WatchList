using Microsoft.EntityFrameworkCore;
namespace WatchList.Models
{
    public class InfoDbContext:DbContext
    {
        public InfoDbContext(DbContextOptions<InfoDbContext> options) : base(options) { }

        public DbSet<SeriesInfo> SeriesInfos =>Set<SeriesInfo>();

        public DbSet<UsersDBModel> UsersDBModels =>Set<UsersDBModel>();

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeriesInfo>()
                .HasOne(s => s.User) // Each SeriesInfo has one User
                .WithMany(u => u.SeriesInfos) // Each User can have many SeriesInfos
                .HasForeignKey(s => s.UserID); // The foreign key is UserID in SeriesInfo
        }


    }
}
