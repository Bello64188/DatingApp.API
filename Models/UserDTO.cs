using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Models
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Please Enter your Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Please Enter Strong Password")]
        public string Password { get; set; }
    }
    public class UserDTO:LoginDTO
    {  
      
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }
        public ICollection<string> Roles { get; set; }
        
        
        
        
    }
}