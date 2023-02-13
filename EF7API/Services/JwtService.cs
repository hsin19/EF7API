using EF7API.Mdoels.Entities;
using EF7API.Services.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EF7API.Services;

public class JwtService
{
    private readonly IOptionsMonitor<MyJwtOptions> _optionsMonitor;

    private MyJwtOptions Options => _optionsMonitor.CurrentValue;

    public JwtService(IOptionsMonitor<MyJwtOptions> options)
    {
        _optionsMonitor = options;
    }

    public string CreateToken(IEnumerable<Claim>? claims = null)
    {
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            NotBefore = DateTime.UtcNow,
            Issuer = Options.Issuer,
            Audience = Options.Audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(Encoding.Default.GetBytes(Options.SignKey)),
            SecurityAlgorithms.HmacSha512Signature)
        };
        if (Options.ExpireTime != null)
        {
            tokenDescriptor.Expires = DateTime.UtcNow.AddMinutes(Options.ExpireTime.Value);
        }
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }

    internal void Configure(JwtBearerOptions options)
    {
        List<string> issuers = new();
        if (Options.Issuer != null)
        {
            issuers.Add(Options.Issuer);
        }
        if (Options.ValidIssuers != null)
        {
            issuers.AddRange(Options.ValidIssuers);
        }
        List<string> audiences = new();
        if (Options.Audience != null)
        {
            audiences.Add(Options.Audience);
        }
        if (Options.ValidAudiences != null)
        {
            audiences.AddRange(Options.ValidAudiences);
        }

        options.SaveToken = true;
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = issuers.Count > 0,
            ValidIssuers = issuers,
            ValidateAudience = audiences.Count > 0,
            ValidAudiences = audiences,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(Options.SignKey)),
            ValidateLifetime = true,
            RequireExpirationTime = Options.ExpireTime != null
        };
    }
}
