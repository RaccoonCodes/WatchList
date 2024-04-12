using Microsoft.EntityFrameworkCore;

namespace WatchList.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            InfoDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<InfoDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.SeriesInfos.Any())
            {
                var user1 = new UsersDBModel
                {
                    UserName = "Mapache",
                    Password = "123"
                };

                var user2 = new UsersDBModel
                {
                    UserName = "Ivan",
                    Password = "456"
                };

                context.UsersDBModels.AddRange(user1, user2);
                context.SaveChanges();

                context.SeriesInfos.AddRange(
                    new SeriesInfo
                    {
                        User = user1,
                        TitleWatched = "Suzume",
                        SeasonWatched = "1",
                        ProviderWatched = "CrunchyRoll",
                        Genre = "Slice of Life"
                    },
                    new SeriesInfo
                    {
                        User = user2,
                        TitleWatched = "The Great Cleric",
                        SeasonWatched = "1",
                        ProviderWatched = "CrunchyRoll",
                        Genre = "Fantasy"
                    }
                );

                context.SaveChanges();
            }
        }

    }
}
