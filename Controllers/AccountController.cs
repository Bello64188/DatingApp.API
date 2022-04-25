namespace DatingApp.API.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using DatingApp.API.Data;
    using DatingApp.API.Models;
    using DatingApp.API.Service;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserApi> _userManager;
        private readonly IAuthManager _authManager;
        private readonly IMapper _map;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<UserApi> userManager, IAuthManager authManager, IMapper mapper, ILogger<AccountController> logger)
        {
            _userManager= userManager;
            _authManager= authManager;
            _map=mapper;
            _logger=logger;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"attempt to resgister{userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var user = _map.Map<UserApi>(userDTO);
            user.UserName= userDTO.Email;
            var result = await _userManager.CreateAsync(user,userDTO.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code,error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRolesAsync(user,userDTO.Roles);
            return Accepted();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation($"Login Attempt for{loginDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (! await _authManager.ValidateUser(loginDTO))
            {
                return Unauthorized();
            }
            return Accepted(new {Token= await _authManager.CreateToken()});
        }
    }
}