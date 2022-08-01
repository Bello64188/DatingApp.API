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
           //member who like me
           if (userParams.Likers)
           {
             var userLikers =  await GetUserLike(userParams.UserId, userParams.Likers);
             users= users.Where(u=>userLikers.Contains(u.Id));
           }
           if (userParams.Likees)
           {
            var userLikees = await GetUserLike(userParams.UserId, userParams.Likers);
            users= users.Where(u=>userLikees.Contains(u.Id));
           }
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

        public async Task<Like> GetLike(string userid, string recipientid)
        {
            return await _context.Like.FirstOrDefaultAsync(u=>u.likerId==userid && u.likeeId==recipientid);
        }
        private async Task<IEnumerable<String>>GetUserLike(string id, bool likers)
        {
            var user = await  _context.UserDatas
            .Include(l=>l.Likees)
            .Include(l=>l.Likers)
            .FirstOrDefaultAsync(i=>i.Id==id);

            if (likers){      

                return user.Likers.Where(i=>i.likerId==id).Select(i=>i.likeeId);
           
            
            }else{
             return user.Likees.Where(i=>i.likeeId==id).Select(i=>i.likerId);
             
            }
        
    }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m=>m.id==id);
        }

        public async Task<PageList<Message>> GetMessageForUser(MessageParams messageParams)
        {
            var message =  _context.Messages
            .Include(m=>m.sender).ThenInclude(m=>m.photos)
            .Include(m=>m.recipient).ThenInclude(m=>m.photos)
            .AsQueryable();
             switch (messageParams.MessageContainer)
             {
                case "inbox":
                message= message.Where(u=>u.recipientId==messageParams.UserId && u.recipientDeleted==false);
                    break;
                case "outbox":
                message = message.Where(u=>u.senderId==messageParams.UserId && u.senderDeleted == false);
                break;    
                default:
                message = message.Where(u=>u.recipientId==messageParams.UserId && u.recipientDeleted== false);
                    break;
                    
             }
              message = message.OrderByDescending(d=>d.messageSent);
              return await PageList<Message>.CreateAsync(message,messageParams.PageNumber,messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(string userid, string recipientid)
        {
            var message = await _context.Messages
            .Include(m=>m.sender).ThenInclude(m=>m.photos)
            .Include(m=>m.recipient).ThenInclude(m=>m.photos)
            .Where(m=>m.senderId==userid && m.recipientId==recipientid && m.senderDeleted==false
             || m.senderId==recipientid && m.recipientId== userid && m.recipientDeleted== false)
             .OrderByDescending(m=>m.messageSent)
             .ToListAsync();
             return message;
        }
    }
}