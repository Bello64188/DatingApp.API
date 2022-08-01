namespace DatingApp.API.Data
{
    public class Like
    {
        public string likerId { get; set; }
        public string likeeId { get; set; }
        public UserData liker { get; set; }
        public UserData likee { get; set; }      
        
        
        
        
    }
}