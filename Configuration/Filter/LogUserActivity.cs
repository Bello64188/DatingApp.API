using System;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using DatingApp.API.IGenericRepository;
using DatingApp.API.Data;

namespace DatingApp.API.Configuration.Filter
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task  OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {   //var user = new UserData();
            var logContext= await next();
           var userId= logContext.HttpContext.User.Claims.FirstOrDefault(c=>c.Type.Equals("id",StringComparison.OrdinalIgnoreCase))?.Value;
            var repo = logContext.HttpContext.RequestServices.GetService<IRepository<UserData>>();
            var resultUser = await repo.GetUser(userId);
            resultUser.lastActive=DateTime.Now;
            await repo.SaveAll();

        }
    }
}