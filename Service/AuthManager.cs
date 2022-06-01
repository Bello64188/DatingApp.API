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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Service
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<UserData> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _cxt;
        private readonly DbSet<UserData> _db;
        private UserData _user;
      
        
        
        

        public AuthManager(UserManager<UserData> userManager, IConfiguration configuration,AppDbContext context)
      {
          _userManager=userManager;
          _configuration= configuration;
          _cxt=context;
          _db=_cxt.Set<UserData>();
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
            var expiration = DateTime.Now.AddDays(Convert.ToDouble(_configuration.GetSection("Lifetime").Value));
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
            var user = new UserData();
            var claim = new List<Claim>{
                new Claim("name", _user.name),
                new Claim("id", _user.Id.ToString()),
                
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
            return (_user != null && await _userManager.CheckPasswordAsync(_user,loginDTO.password));
        }
    }
}