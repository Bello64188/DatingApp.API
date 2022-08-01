using System.Collections.Generic;
namespace DatingApp.API.Configuration.Filter
{
  public class UserParams
{
    public int PageNumber { get; set; }=1;
    private int pageSize=10;
    public int PageSize
    {
        get { return pageSize; }
        set { pageSize = (value>MaxPageSize? MaxPageSize: value); }
    }

    private const int MaxPageSize =50;
    public string UserId { get; set; }   
    
    public int MinAge { get; set; } = 18;
    public int MaxAge { get; set; }= 99;
    public string Gender { get; set; }   
    
    public string OrderBy { get; set; }
    public bool Likers { get; set; } = false;
    
    public bool Likees { get; set; } = false;
    
    
    
    
    
}  
}
