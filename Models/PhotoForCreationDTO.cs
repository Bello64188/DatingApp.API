using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Models
{
    public class PhotoForCreationDTO
    {
        public string url { get; set; }
        public IFormFile file { get; set; }
        public string description { get; set; }
        public DateTime dateAdd { get; set; }
        public string publicId { get; set; }
        public PhotoForCreationDTO()
        {
            dateAdd= DateTime.Now;
        }
        
        
        
        
        
    }
}