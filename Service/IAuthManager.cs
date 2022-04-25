using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Service
{
    public interface IAuthManager
    {
         public Task<bool> ValidateUser(LoginDTO loginDTO);
         public Task<string> CreateToken();
        
         
             
         
    }
}