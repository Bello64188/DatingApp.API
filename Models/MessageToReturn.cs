using System;

namespace DatingApp.API.Models
{
    public class MessageToReturn
    {
        
         public int id { get; set; }
        public string  senderId { get; set; }
        public  string  senderKnownAs { get; set; }
        public string senderPhotoUrl { get; set; }       
        public string  recipientId { get; set; }
        public  string recipientKnownAs { get; set; }
        public string recipientPhotoUrl { get; set; }          
        public string content { get; set; }
        public bool isRead { get; set; }
        public DateTime? dateRead { get; set; }
        public DateTime messageSent { get; set; }
    }
}