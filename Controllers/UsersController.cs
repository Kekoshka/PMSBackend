using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Context;

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
            return user is null ? NotFound() : Ok(user);
        }
    }
}
