using System;

namespace DatingApp.API.Data
{
    public class Photo
    {
        public int id { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public DateTime dateAdd { get; set; }
        public bool isMain { get; set; }
        public string publicId { get; set; }       
        
        public UserData userData { get; set; }
        public string userDataId { get; set; }
        
        
        
        
        
        
        

        
        
        
        
        
        

    }
}