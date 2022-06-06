using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public class UserDTO:LoginDTO
    {  
      
        public string name { get; set; }
        public string gender { get; set; }         
        public DateTime dateOfBirth { get; set; }
        public string knownAs { get; set; }        
        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }       
        public string interests { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string phoneNumber { get; set; }
        public ICollection<string> Roles { get; set; }
        
        public UserDTO()
        {
            created= DateTime.Now;
            lastActive= DateTime.Now;
        }
        
        
    }
}