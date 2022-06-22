using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Data
{
    public class UserData:IdentityUser
    {

        public UserData()
        {
          photos= new Collection<Photo>();
        }
       
        public string name { get; set; }        
                
        public string gender { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string knownAs { get; set; }        
        public DateTime created { get; set; }
        public DateTime lastActive { get; set; }
        public string introduction { get; set; }
        public string lookingFor { get; set; }
        public string interests { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        
        [NotMapped]
        public ICollection<string> Roles { get; set; }
        public ICollection<Photo> photos { get; set; } 
       
        
           
        
        
        
    }
    
}