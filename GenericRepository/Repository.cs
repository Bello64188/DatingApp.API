using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Configuration.Filter;
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

        public async Task<PageList<UserData>> GetUsers(UserParams userParams)
        {
           var users= _context.UserDatas.Include(p=>p.photos).OrderByDescending(u=>u.lastActive).AsQueryable();
           #region "filtering"
            users = users.Where(u=>u.Id != userParams.UserId);
           users = users.Where(g=>g.gender==userParams.Gender);
           if (userParams.MinAge!=18 || userParams.MaxAge!=99)
           {
            DateTime maxDob = DateTime.Today.AddYears(-userParams.MinAge);
            DateTime minDob = DateTime.Today.AddYears(-userParams.MaxAge-1);
            users=users.Where(u=>u.dateOfBirth>=minDob 
            && u.dateOfBirth <= maxDob);
           }
           #endregion "filtering"
           #region "sorting"
           if (!string.IsNullOrEmpty(userParams.OrderBy))
           {
              switch (userParams.OrderBy.ToLower())
              {
                case "created":
                users.OrderByDescending(u=>u.created);
                    break;
                default:
                users.OrderByDescending(u=>u.lastActive);
                    break;
              }
              
           }
           #endregion "sorting"
          return await PageList<UserData>.CreateAsync(users,userParams.PageNumber,userParams.PageSize);
        }
    }
}