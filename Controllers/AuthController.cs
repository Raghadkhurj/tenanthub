using Microsoft.AspNetCore.Mvc;
using TenantHub.Data;
using TenantHub.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TenantHub.Services;

namespace TenantHub.Controllers {
    [ApiController]
    [Route ("[controller]")]

    public class AuthController: ControllerBase{
        private readonly JwtTokenService _tokenService;
        private readonly AppDbContext _context;
        public AuthController(AppDbContext context,JwtTokenService tokenService){
            _context=context;
           _tokenService=tokenService;

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
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
            new Claim(ClaimTypes.Email, admin.Email),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var token =_tokenService.CreateToken(claims);
       return Ok(new{token});
         }
   
        [HttpPost("user/login")]
    public async Task<IActionResult> UserLogin(LoginRequest request)
    {
        var tenant = await _context.Tenants.FirstOrDefaultAsync(t => t.Slug == request.CompanySlug);
        if (tenant == null)
        {
            return Unauthorized("Invalid email or password");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.TenantId == tenant.Id && u.Email == request.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid email or password");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User"),
            new Claim("TenantId", user.TenantId.ToString())
        };
     var token =_tokenService.CreateToken(claims);
       return Ok(new {token});
         
    }
   [HttpGet("/current-user")]
   [Authorize(Roles = "User")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var tenantId = int.Parse(User.FindFirst("TenantId")!.Value);

        var user = await _context.Users
            .Include(u => u.Tenant)
            .FirstOrDefaultAsync(u => u.Id == userId && u.TenantId == tenantId);

        if (user == null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            user.Id,
            user.Email,
            user.CreatedDate,
            Tenant = new { user.Tenant.Id, user.Tenant.Name, user.Tenant.Slug }
        });
    }

   }
}