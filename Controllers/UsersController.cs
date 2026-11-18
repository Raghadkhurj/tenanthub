using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenantHub.Data;
using TenantHub.Dtos;
using TenantHub.Models;

namespace TenantHub.Controllers {
    [ApiController]
    [Route ("[controller]")]
    [Authorize(Roles="Admin")]
    public class UsersController: ControllerBase{        
        private readonly AppDbContext _context;
        public UsersController(AppDbContext context){
            _context=context;
        }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            var tenant=await _context.Tenants.FirstOrDefaultAsync(t=>t.Slug==request.CompanySlug);
            if (tenant == null)
            {
                return BadRequest("Company not found");
            }
            var emailExists = await _context.Users.AnyAsync(u => u.TenantId == tenant.Id && u.Email == request.Email);

            if (emailExists)
            {
                return Conflict("Email already exists in this company");
            }
            var user=new User
            {
                Email=request.Email,
                PasswordHash=BCrypt.Net.BCrypt.HashPassword(request.Password),
                TenantId=tenant.Id
            };
            _context.Users.Add(user);             
            await _context.SaveChangesAsync(); 
          return Ok(new { user.Id, user.Email, user.TenantId });
            }
    

    }
}
