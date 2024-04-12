using System.Drawing;

namespace WatchList.Models
{
    public class FinalViewModel
    {
        public LoginModel LoginModel { get; set; } = new LoginModel();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
        public IInfoRepository? InfoRepository { get; set; }
        public IEnumerable<SeriesInfo>? FilteredSeries {  get; set; }
        public string? SelectedGenre { get; set; }

    }
}
