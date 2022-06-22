using System.Collections.Generic;
using System.Linq;
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

       
        public async Task<Photo> GetMainPhotoForUser(string userid)
        {
           var user = await _context.Photos.Where(u=>u.userDataId==userid).FirstOrDefaultAsync(i=>i.isMain);
           return user;
        }

        public Task<Photo> GetPhoto(int id)
        {
            var photo = _context.Photos.FirstOrDefaultAsync(u=>u.id == id);
                  return photo;
        }       

        public async Task<UserData> GetUser(string id)
        {
            var user = await _context.UserDatas.Include(p=>p.photos).FirstOrDefaultAsync(i=>i.Id==id);
            return user;
        }
        public async Task<UserData> GetEmail(string email)
        {
             var user = await _context.UserDatas.Include(p=>p.photos).FirstOrDefaultAsync(i=>i.Email==email);
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

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public async Task<UserData> Login(string email, string password)
        {
            var user = await _context.UserDatas.Include(p=>p.photos).FirstOrDefaultAsync(h=>h.Email==email);
            if (user==null)
            {
                return null;

            }
            return user;
        }
    }
}