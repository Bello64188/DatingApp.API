using System;

namespace DatingApp.API.Models
{
    public class MessageForCreateDto
    {
        public string senderId { get; set; }
        public string senderKnownAs { get; set; }
        public string recipientId { get; set; }
        public string content { get; set; }
        public DateTime messageSent { get; set; }
        public MessageForCreateDto()
        {
            messageSent= DateTime.Now;
        }
        
        
        
        
        
    }
}