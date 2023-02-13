using EF7API.Mdoels.Entities;
using EF7API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EF7API.Controllers;

public record LoginVM(string UserId, string Password);

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly BlogDBContext _context;
    private readonly JwtService _jwtService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(BlogDBContext context, JwtService jwtService, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public IActionResult CreateToken(LoginVM loginVM)
    {
        User? user = _context.Users
            .Where(e => e.UserId == loginVM.UserId)
            .SingleOrDefault();

        if (user == null)
        {
            return Unauthorized();
        }
        else if (_passwordHasher.VerifyHashedPassword(user, user.Password, loginVM.Password) == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.UserId)
        };
        string token = _jwtService.CreateToken(claims);
        return Ok(token);
    }

    [HttpGet("claims")]
    [Authorize]
    public IActionResult GetClaims()
    {
        return Ok(User.Claims.Select(p => new { p.Type, p.Value }));
    }
}
