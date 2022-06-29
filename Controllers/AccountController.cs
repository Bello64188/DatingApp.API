namespace DatingApp.API.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using DatingApp.API.Data;
    using DatingApp.API.IGenericRepository;
    using DatingApp.API.Models;
    using DatingApp.API.Service;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]"), Produces("application/json")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserData> _userManager;
        private readonly IAuthManager _authManager;
        private readonly IMapper _map;
        private readonly ILogger<AccountController> _logger;
        private readonly IRepository<UserData> _cxt;
        

        public AccountController(UserManager<UserData> userManager, 
        IAuthManager authManager, IMapper mapper, ILogger<AccountController> logger, IRepository<UserData> context)
        {
            _userManager= userManager;
            _authManager= authManager;
            _map=mapper;
            _logger=logger;
            _cxt=context;
        
        }
       [HttpPost]
       [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"attempt to resgister{userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var user = _map.Map<UserData>(userDTO);
            user.UserName= userDTO.Email.ToLower();            
            var result = await _userManager.CreateAsync(user,userDTO.password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code,error.Description);
                }
                return BadRequest(ModelState);
            }
            // await _userManager.AddToRolesAsync(user,userDTO.Roles);
            var returnToUser = _map.Map<UsersDetailsDTO>(user);
            return CreatedAtRoute("GetUser", new {controller="User", id = user.Id},returnToUser); 
        }

        [HttpPost("login")]
       
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
           var user= await _cxt.Login(loginDTO.Email,loginDTO.password);
             
            _logger.LogInformation($"Login Attempt for{loginDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _authManager.ValidateUser(loginDTO))
            {
                return Unauthorized();
            }
             if (user==null)
             {
             return Unauthorized();
             }
                       
            var userfrom = _map.Map<UserListsDto>(user);
            return Ok(new {
                Token= await _authManager.CreateToken(),
                userfrom = userfrom
                });
        }
    }
}