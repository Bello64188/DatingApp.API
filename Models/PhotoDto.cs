using System;

namespace DatingApp.API.Models
{
    public class PhotoDto
    {
        public int id { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public DateTime dateAdd { get; set; }
        public bool isMain { get; set; }        
      
    }
}