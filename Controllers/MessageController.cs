using System;
using System.Linq;
namespace DatingApp.API.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using DatingApp.API.Configuration.Filter;
    using DatingApp.API.Data;
    using DatingApp.API.IGenericRepository;
    using DatingApp.API.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
        [ServiceFilter(typeof(LogUserActivity))]
        [Route("api/user/{userid}/[controller]")]
        [ApiController]
        [Authorize]
        public class MessageController : ControllerBase
        {
        private IMapper _map;
        private IRepository<Message> _repository;
        private IRepository<UserData> _userRepository;

        public MessageController(IMapper mapper,
         IRepository<Message> repository, 
        IRepository<UserData> userRepo)
            {
                _map=mapper;
                _repository=repository;
                _userRepository = userRepo;
            }
            [HttpGet("{id}", Name="GetMessage")]
            public async Task<IActionResult> GetMessage(string userid, int id)
            {
                if(userid!= User.Claims.FirstOrDefault(m=>m.Type.Equals("id",StringComparison.OrdinalIgnoreCase)).Value)
                return Unauthorized();
                var message = await _repository.GetMessage(id);
                if(message==null) return NotFound();
                return Ok(message);
            }
            [HttpGet]
            public async Task<ActionResult> GetMessageForUser([FromQuery]  MessageParams messageParams, string userid )
           {
                if(userid!= User.Claims.FirstOrDefault(m=>m.Type.Equals("id",StringComparison.OrdinalIgnoreCase)).Value)
                return Unauthorized();
                messageParams.UserId=userid;
                 var messageParamFroRepo= await _repository.GetMessageForUser(messageParams);
                 var message = _map.Map<IEnumerable<MessageToReturn>>(messageParamFroRepo);
                 Response.AddPagination(messageParamFroRepo.CurrentPage,
                 messageParamFroRepo.PageSize, messageParamFroRepo.TotalCount
                 ,messageParamFroRepo.TotalPage);

                 return Ok(message);
            }
            [HttpGet("thread/{id}")]
            public async Task<IActionResult> GetMessageThread(string userid, string id)
            {
                if(userid!= User.Claims.FirstOrDefault(m=>m.Type.Equals("id",StringComparison.OrdinalIgnoreCase)).Value)
                return Unauthorized();
                var messageParamFroRepo = await _repository.GetMessageThread(userid,id);
                var messageThread = _map.Map<IEnumerable<MessageToReturn>>(messageParamFroRepo);
                return Ok(messageThread);
                
            }
           [HttpPost]
           public async Task<IActionResult> CreateMessage([FromBody] MessageForCreateDto messageForCreate,
                                                    string userid)
           {
            var sender = await _userRepository.GetUser(userid);
            if(sender.Id!= User.Claims.FirstOrDefault(u=>u.Type.Equals("id",StringComparison.OrdinalIgnoreCase)).Value)
            return Unauthorized();
             messageForCreate.senderId=userid;
            
            var recipient = await _repository.GetUser(messageForCreate.recipientId);
            if(recipient==null) return BadRequest("Cloud not find user.");
            var message = _map.Map<Message>(messageForCreate);
            _repository.Add(message);
            if (await _repository.SaveAll())
            {
                var messageToReturn = _map.Map<MessageToReturn>(message);
                return CreatedAtRoute("GetMessage", new {userid=userid,id=message.id},messageToReturn);
            }
            throw new Exception("Creating the message failed on save");
           }
        }
    
}