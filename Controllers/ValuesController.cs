using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{  

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ValuesController(AppDbContext context)
        {
            _context=context;
        }
        [HttpGet]
    
        public async Task< IActionResult> GetValues()
        {
            var values = await  _context.Values.ToListAsync();
            return Ok(values);            
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult>  Get(int id)
        {
            var value= await _context.Values.Where(i=>i.Id==id).FirstOrDefaultAsync();
            return Ok(value);
        }

    }
}