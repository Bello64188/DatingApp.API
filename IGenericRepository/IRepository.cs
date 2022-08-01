using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Configuration.Filter;
using DatingApp.API.Data;

namespace DatingApp.API.IGenericRepository
{
    public interface IRepository<T> where T :class
    {
         Task<PageList<UserData>> GetUsers(UserParams userParams);
         Task<UserData> GetUser(string id);
         Task<UserData> GetEmail(string email);
         Task<UserData> Login(string email, string password);
         void Add(T entity);
         void Delete(T entity);
         void Update(T entity);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(string userid);
         Task<Like> GetLike(string userid, string recipientid);
         Task<Message> GetMessage(int  id);
         Task<PageList<Message>> GetMessageForUser(MessageParams messageParams);
         Task<IEnumerable<Message>> GetMessageThread(string userid, string recipientid);     
            
         Task<bool>  SaveAll();
    }
}