using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Service
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<UserApi> _userManager;
        private readonly IConfiguration _configuration;
        private UserApi _user;
        
        
        

        public AuthManager(UserManager<UserApi> userManager, IConfiguration configuration)
      {
          _userManager=userManager;
          _configuration= configuration;
      }
        
        
        public async Task<string> CreateToken()
        {
           var claim = await GetClaims();
           var signingCredential= GetSigningCredentials();
           var token = GetTokenOption(signingCredential,claim);
           return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GetTokenOption(SigningCredentials signingCredential, List<Claim> claim)
        {
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration.GetSection("Lifetime").Value));
            var jwtsetting = _configuration.GetSection("Jwt");
            var token = new JwtSecurityToken(

                issuer:jwtsetting.GetSection("validIssuer").Value,
                claims:claim,
                expires:expiration,
                signingCredentials:signingCredential
            );
              return token;

        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Environment.GetEnvironmentVariable("KEYAPI");
            var secret= new SymmetricSecurityKey(Encoding.UTF8.GetBytes((key)));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256 );
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claim = new List<Claim>{
                new Claim(ClaimTypes.Name, _user.UserName)
            };
             var userRoles = await _userManager.GetRolesAsync(_user);
             foreach (var role in userRoles)
             {
                 claim.Add(new Claim(ClaimTypes.Role,role));
             }
             return claim;
        }

        public async Task<bool> ValidateUser(LoginDTO loginDTO)
        {
            _user= await _userManager.FindByNameAsync(loginDTO.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user,loginDTO.Password));
        }
    }
}