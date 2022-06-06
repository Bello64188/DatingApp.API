using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;

namespace DatingApp.API.IGenericRepository
{
    public interface IRepository<T> where T :class
    {
         Task<IEnumerable<UserData>> GetUsers();
         Task<UserData> GetUser(string id);
         void Add(T entity);
         void Delete(T entity);
         void Update(T entity);
         Task<bool>  SaveAll();
    }
}