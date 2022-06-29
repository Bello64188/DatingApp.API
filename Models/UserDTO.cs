using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Models
{
    public class UserDTO:LoginDTO
    {  
       [Required]
        public string name { get; set; }
        [Required]
        public string gender { get; set; }
        [Required]        
        public DateTime dateOfBirth { get; set; }
        [Required]
        public string knownAs { get; set; }  

        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }       
        // public string interests { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        public ICollection<string> Roles { get; set; }
        
        public UserDTO()
        {
            created= DateTime.Now;
            lastActive= DateTime.Now;
        }
        
        
    }
}