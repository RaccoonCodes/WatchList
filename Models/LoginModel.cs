using System.ComponentModel.DataAnnotations;

namespace WatchList.Models
{
    public class LoginModel
    {
        public int? UserID { get; set; } 

        [Required(ErrorMessage ="Please enter your Username")]
        public string? LoginName {  get; set; }

        [Required(ErrorMessage ="Please enter your Password")]
        public string? Password { get; set; } 
    }
}
