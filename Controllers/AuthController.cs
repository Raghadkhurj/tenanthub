using Microsoft.AspNetCore.Mvc;
using TenantHub.Data;
using TenantHub.Dtos;
using Microsoft.EntityFrameworkCore;

namespace TenantHub.Controllers {
    [ApiController]
    [Route ("[controller]")]

    public class AuthController: ControllerBase{

        private readonly AppDbContext _context;
        public AuthController(AppDbContext context){
            _context=context;

        }
        
    [HttpPost("admin/Login")]
     public async Task<IActionResult> Login(AdminLoginRequest adminLogin) {
            var admin = await _context.Admins.FirstOrDefaultAsync
            (a => a.Email == adminLogin.Email);
            if(admin==null){
                return Unauthorized("Invalid email or password ");

            }
           if(!BCrypt.Net.BCrypt.Verify(adminLogin.Password,admin.PasswordHash)){
                return Unauthorized("Invalid email or password ");

           }
           return Ok("Login successful");
            }
    }

}