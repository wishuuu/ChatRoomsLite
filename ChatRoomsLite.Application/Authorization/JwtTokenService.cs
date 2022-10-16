using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatRoomsLite.Models.Entities.User;
using Microsoft.IdentityModel.Tokens;

namespace ChatRoomsLite.Application.Authorization;

public class JwtTokenService : ITokenService
{
    public string CreateToken(User user)
    {
        string secretKey = "eyJhbGciOiJIUzI1NiJ9.ew0KICAic3ViIjogIjEyMzQ1Njc4OTAiLA0KICAibmFtZSI6ICJBbmlzaCBOYXRoIiwNCiAgImlhdCI6IDE1MTYyMzkwMjINCn0.0qnqLbn5A-pTPqD8k8Y-f0U6bo_tzF53ktJ-rpH41Ws";
        string issuer = "ChatRooms";

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Username),
            new (ClaimTypes.Role, "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials
        );
        
        var handler = new JwtSecurityTokenHandler();
        
        return handler.WriteToken(token);
    }
}

public interface ITokenService
{
    string CreateToken(User user);
}