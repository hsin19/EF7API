using EF7API.Mdoels.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EF7API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InitController : ControllerBase
{
    private const string AdminId = "admin";
    private const string AdminPwd = "admin";

    private readonly BlogDBContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public InitController(BlogDBContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpPost]
    public async Task<ActionResult<User>> InitSystem(LoginVM loginVM)
    {
        if (loginVM.UserId == AdminId && loginVM.Password == AdminPwd)
        {
            if (!await _context.Users.AnyAsync())
            {
                User adminUser = new()
                {
                    UserId = "admin",
                    UserName = "Administrator",
                    Password = "0000"
                };
                adminUser.Password = _passwordHasher.HashPassword(adminUser, "0000");
                _context.Users.Add(adminUser);
                await _context.SaveChangesAsync();
                adminUser.Password = "0000";
                return Ok(adminUser);
            }
        }
        return Unauthorized();
    }
}
