using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly AppDbContext _context;
        public Seed(AppDbContext context)
        {
            _context = context;

        }
        public void SeedUsers(){
        //    // clear database
        //     _context.RemoveRange(_context.UserDatas);
        //     _context.SaveChanges();
            //seed
            var userData = File.ReadAllText("Data/UserDataSeed.json");
            var users = JsonConvert.DeserializeObject<List<UserData>>(userData);
            foreach (var user in users)
            {
                user.name=user.name.ToLower();
                _context.Add(user);
            }
            _context.SaveChanges();
        }
        
    }
}