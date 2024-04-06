using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoggerMicroservice.Services.Contracts;
using LoggerMicroservice.Settings;
using Microsoft.IdentityModel.Tokens;

namespace LoggerMicroservice.Services;

public class JwtService: IJwtService
{
    private IAppSettingsReader appSettingsReader;
    
    public JwtService(IAppSettingsReader appSettingsReader)
    {
        this.appSettingsReader = appSettingsReader;
    }
    

    private SecurityTokenDescriptor GenerateTokenDescriptor(List<Claim> claims)
    {
        var key = Encoding.ASCII.GetBytes(appSettingsReader.GetValue(AppSettingsConstants.Section.Authorization,AppSettingsConstants.Keys.JwtSecretKey));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenDescriptor;
    }
   

    public string GetUserEmailFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken == null)
        {
            throw new ArgumentException("Invalid JWT token");
        }

        var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);

        return claim?.Value;
    }
}