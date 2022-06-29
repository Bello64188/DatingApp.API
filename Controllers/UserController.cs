using System.Linq;
namespace Name.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using DatingApp.API;
    using DatingApp.API.Configuration.Filter;
    using DatingApp.API.Data;
    using DatingApp.API.IGenericRepository;
    using DatingApp.API.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IRepository<UserData> _repository;
        private readonly IMapper _map;

        public UserController(IRepository<UserData> repository, IMapper mapper){ 
            _repository = repository;
            _map=mapper;
            }

        [HttpGet(Name ="GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentId = User.Claims.FirstOrDefault(c=>c.Type.Equals("id",StringComparison.OrdinalIgnoreCase))?.Value;
            var userfromrepo = await _repository.GetUser(currentId);
            userParams.UserId=currentId;
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender= userfromrepo.gender.ToLower() == "male" ? "female": "male";
            }
            var users= await _repository.GetUsers(userParams);
            var usersMap = _map.Map<IList<UserListsDto>>(users);
            Response.AddPagination(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPage);
            return Ok(usersMap);
        }
        [HttpGet("{id}",Name ="GetUser")]
       
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _repository.GetUser(id);
            var userMap = _map.Map<UsersDetailsDTO>(user);

            return Ok(userMap);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(statusCode:StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode:StatusCodes.Status400BadRequest)]
       public async Task<IActionResult> UpDateUser([FromBody] UpdateDTO update , string id){
         if (!ModelState.IsValid || string.IsNullOrEmpty(id))
         
             return BadRequest(ModelState);
         
           var user = await _repository.GetUser(id);
           if (user==null)
           
               return BadRequest($"User of id {id} is not found");
           
           _map.Map(update,user);
           _repository.Update(user);
           await _repository.SaveAll();
           return NoContent();
        }
}
}