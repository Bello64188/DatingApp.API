using System;

namespace DatingApp.API.Data
{
    public class Message
    {
        public int id { get; set; }
        public string  senderId { get; set; }
        public virtual UserData  sender { get; set; }
        public string  recipientId { get; set; }
        public virtual UserData recipient { get; set; }
        public string content { get; set; }
        public bool isRead { get; set; }
        public DateTime? dateRead { get; set; }
        public DateTime messageSent { get; set; }
        public bool senderDeleted { get; set; }
        public bool recipientDeleted { get; set; } 
        
        
        
    }
}