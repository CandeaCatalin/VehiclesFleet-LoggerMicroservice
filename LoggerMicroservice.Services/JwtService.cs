using System.IdentityModel.Tokens.Jwt;
using LoggerMicroservice.Services.Contracts;

namespace LoggerMicroservice.Services;

public class JwtService: IJwtService
{
    private IAppSettingsReader appSettingsReader;
    
    public JwtService(IAppSettingsReader appSettingsReader)
    {
        this.appSettingsReader = appSettingsReader;
    }
    
    public string GetUserEmailFromToken(string? token)
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