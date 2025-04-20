using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PMSBackend.Common;
using PMSBackend.Context;
using System.IdentityModel.Tokens.Jwt;

namespace PMSBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        ApplicationContext _context;
        public UsersController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet("{login}/{password}")]
        public async Task<IActionResult> login(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Mail == login && u.Password == password);
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: user.Id.ToString(),
                claims: null,
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return user is null ? Unauthorized("Login or password is incorrect") : Ok(new JwtSecurityTokenHandler().WriteToken(jwt));

        }
    }
}
