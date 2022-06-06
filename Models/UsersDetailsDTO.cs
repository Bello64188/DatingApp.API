using System;
using System.Collections.Generic;
using DatingApp.API.Data;

namespace DatingApp.API.Models
{
    public class UsersDetailsDTO
    {
        public string Id { get; set; }       
        
        public string Email { get; set; }  
        public string name { get; set; }                
        public string gender { get; set; }
        public int age { get; set; }
        public string knownAs { get; set; }        
        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }
        public string introduction { get; set; }
        public string lookingFor { get; set; }
        public string phoneNumber { get; set; }
        public string interests { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string photoUrl { get; set; }
        
        
        public ICollection<PhotoDto> photos { get; set; }
        
    }
}