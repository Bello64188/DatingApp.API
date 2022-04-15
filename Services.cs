using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API
{
    public  static class Services
    {
        public static void CorsConfiguration( this IServiceCollection services){
         
         services.AddCors(op=>op.AddPolicy("policy",p=>{
            p.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
            
            
         }));
        }
    }
}