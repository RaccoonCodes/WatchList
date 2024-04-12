namespace WatchList.Models
{
    public interface IInfoRepository
    {
        IQueryable<SeriesInfo> SeriesInfos { get; }

        void SaveInfo(SeriesInfo series);
        void CreateInfo(SeriesInfo series);
        void DeleteInfo(SeriesInfo series);

        SeriesInfo? GetSeriesInfoByID(int seriesInfoID, bool includeUser = false);

        SeriesInfo? GetSeriesInfoByAttributes(SeriesInfo seriesInfo);

        int GetLastSeriesInfoID();
        int GetTotalItemCount(int userID);
    }
}
