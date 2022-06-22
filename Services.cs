using System;
using System.Text;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
{
    public  static class Services
    {
        public static void CorsConfiguration( this IServiceCollection services){
         
         services.AddCors(op=>op.AddPolicy("policy",p=>{
            p.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
            
            
            
         }));
        }

        public static void IdentityConfiguration(this IServiceCollection services){
       var builder= services.AddIdentityCore<UserData>(op=>op.User.RequireUniqueEmail=true);
       builder= new IdentityBuilder(builder.UserType,typeof(IdentityRole),services);
       builder.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }

        public static void JwtConfiguration(this IServiceCollection services, IConfiguration configuration)

        {
            var key = Environment.GetEnvironmentVariable("KEYAPI");
            var jwtSetting= configuration.GetSection("Jwt");
            services.AddAuthentication(option=>{
                option.DefaultAuthenticateScheme= JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option=>{
                option.TokenValidationParameters= new TokenValidationParameters
                    { 
                    ValidateIssuer = false,
                    ValidateAudience=false,
                    ValidateLifetime= true,
                    ValidateIssuerSigningKey= true,
                    ValidIssuer = jwtSetting?.GetSection("Issuer").Value,
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    
                    
                    };
               
            });
        }

        public static int CalculateAge(this DateTime theAge){
              var age = DateTime.Today.Year- theAge.Year;
            if (theAge.AddYears(age)> DateTime.Today)
            
                 age--;
                 return age;
            
        }
    }
}