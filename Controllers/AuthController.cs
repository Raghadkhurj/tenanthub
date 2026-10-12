using Microsoft.AspNetCore.Mvc;
using TenantHub.Data;

namespace TenantHub.Controllers {
    [ApiController]
    [Route ("[controller]")]

    public class AuthController: ControllerBase{

        private readonly AppDbContext _context;
        public AuthController(AppDbContext context){
            _context=context;

        }
        
         
    // [HttpPost("Login")]
    // public 
    }

}