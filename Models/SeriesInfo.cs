using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchList.Models
{
    public class SeriesInfo
    {
        [Key]
        public int SeriesInfoID {  get; set; }
        public int UserID { get; set; } //Foreign Key 
        public UsersDBModel? User { get; set; } //Navigaton, allow access related UsersDBModel objects.

        [Required(ErrorMessage ="Please enter Title")]
        public string TitleWatched { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Season")]
        [RegularExpression(@"^\d+$",ErrorMessage ="Season must contain only whole numbers")]
        public string SeasonWatched { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter Provider")]
        public string ProviderWatched { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the Genre of the series")]
        public string Genre { get; set; } = string.Empty;

    }
}
