using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Data
{
    public class UserApi:IdentityUser
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
            
    }
}