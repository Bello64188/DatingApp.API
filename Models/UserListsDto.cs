using System;
using System.Collections.Generic;
using DatingApp.API.Data;

namespace DatingApp.API.Models
{
    public class UserListsDto
    {
        public string Id { get; set; }       
        
        public string Email { get; set; }  
        public string name { get; set; }           
        public string gender { get; set; }
        public int age { get; set; }
        public string knownAs { get; set; }  
        public string phoneNumber { get; set; }      
        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string photoUrl { get; set; }
        
        
      
    }
}