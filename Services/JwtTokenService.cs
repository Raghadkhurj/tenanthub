using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
namespace TenantHub.Services{
public class JwtTokenService
{
    public  SymmetricSecurityKey SigningKey{get;}
    public JwtTokenService(IConfiguration config)
    {
        var jwtKey=config["JWT_KEY"];
         if (string.IsNullOrWhiteSpace(jwtKey) )
            {
                throw new InvalidOperationException("JWT_KEY is missing!");
            }
            if (Encoding.UTF8.GetBytes(jwtKey).Length<32)
            {
            throw new InvalidOperationException("JWT_KEY must be at least 32 bytes long");

            }
        SigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    }
    public string CreateToken(IEnumerable<Claim> claims)
    {
        var creds = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);
      return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
}