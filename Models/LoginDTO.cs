using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Please Enter your Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Please Enter Strong Password")]
        public string password { get; set; }
    }
}