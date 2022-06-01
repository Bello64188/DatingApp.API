using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.IGenericRepository;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context=context;
            
        }
        public void Add(T entity)
        {
           _context.Add(entity);
        }

        public void Delete(T entity)
        {
          _context.Remove(entity);
        }

        public async Task<UserData> GetUser(string id)
        {
            var user = await _context.UserDatas.Include(p=>p.photos).FirstOrDefaultAsync(i=>i.Id==id);
            return user;
        }

        public async Task<IEnumerable<UserData>> GetUsers()
        {
          var users=await _context.UserDatas.Include(p=>p.photos).ToListAsync();
          return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync()> 0;
        }
    }
}