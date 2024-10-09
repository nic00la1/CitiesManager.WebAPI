using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames =
    System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace CitiesManager.Core.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthenticationResponse GenerateJwtToken(ApplicationUser user)
    {
        DateTime expiration = DateTime.UtcNow.AddMinutes(
            Convert.ToDouble(_configuration["Jwt: EXPIRATION_MINUTES"]));

        Claim[] claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()), // JWT unique ID
            new Claim(JwtRegisteredClaimNames.Iat,
                DateTime.UtcNow
                    .ToString()), // Issued at (date and time of token generation)
            new Claim(JwtRegisteredClaimNames.NameId,
                user.Email), // Email of the user (Unique Identifier)
            new Claim(JwtRegisteredClaimNames.Name,
                user.PersonName) // Name of the user
        };

        SymmetricSecurityKey securityKey = new(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        SigningCredentials signingCredentials =
            new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken tokenGenerator = new(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"], claims, expires : expiration,
            signingCredentials : signingCredentials);

        JwtSecurityTokenHandler tokenHandler = new();

        string token = tokenHandler.WriteToken(tokenGenerator);

        return new AuthenticationResponse()
        {
            Token = token,
            Email = user.Email,
            PersonName = user.PersonName,
            Expiration = expiration
        };
    }
}
