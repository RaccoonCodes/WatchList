using Microsoft.EntityFrameworkCore;

namespace WatchList.Models
{
    public class EFInfoRepository : IInfoRepository
    {
        private InfoDbContext context;

        public EFInfoRepository(InfoDbContext context) {  
            this.context = context; 
        }

       public IQueryable<SeriesInfo> SeriesInfos => context.SeriesInfos;

        
        public void SaveInfo(SeriesInfo series) { 
            context.Update(series);
            context.SaveChanges();
        }

        public void DeleteInfo(SeriesInfo series) {  
            context.Remove(series); 
            context.SaveChanges();
        }

        public void CreateInfo(SeriesInfo series) {  
            context.Add(series);
            context.SaveChanges();
        }
        
        public int GetTotalItemCount(int userID)
        {
            return context.SeriesInfos.Count(s => s.UserID == userID);
        }

        public int GetLastSeriesInfoID()
        {
            int lastSeriesInfoID = context.SeriesInfos
                .OrderByDescending(s => s.SeriesInfoID).Select(s => s.SeriesInfoID).FirstOrDefault();
            return lastSeriesInfoID;        
        }

        public SeriesInfo? GetSeriesInfoByID(int seriesInfoID, bool includeUser = false)
        {
            if (includeUser)
            {
                return context.SeriesInfos
                    .Include(s => s.User)
                    .FirstOrDefault(s => s.SeriesInfoID == seriesInfoID);
            }
            else
            {
                return context.SeriesInfos
                    .FirstOrDefault(s => s.SeriesInfoID == seriesInfoID);
            }
        }

        public SeriesInfo? GetSeriesInfoByAttributes(SeriesInfo seriesInfo)
        {
            return context.SeriesInfos
           .FirstOrDefault(s =>
               s.TitleWatched == seriesInfo.TitleWatched &&
               s.SeasonWatched == seriesInfo.SeasonWatched &&
               s.ProviderWatched == seriesInfo.ProviderWatched &&
               s.Genre == seriesInfo.Genre &&
               s.UserID == seriesInfo.UserID);
        }

    }
}
