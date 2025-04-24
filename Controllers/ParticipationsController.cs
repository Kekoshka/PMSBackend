using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Context;
using PMSBackend.Models;
using System.Linq;
using System.Security.Claims;

namespace PMSBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipationsController : ControllerBase
    {
        ApplicationContext _context;
        public ParticipationsController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int projectId)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _context.Participations.FirstOrDefaultAsync(p => p.UserId == userId && p.ProjectId == projectId);
            if (user is null) return Unauthorized("User isn't a member of the project");
            var participations = await _context.Participations.Where(p => p.ProjectId == projectId).ToListAsync();
            return Ok(participations);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Participation participation)
        {
            int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier));
            var user = await _context.Participations.FirstOrDefaultAsync(p => p.UserId == UserId && p.RoleId == 1 && p.ProjectId == participation.ProjectId);
            if (user is null) return Unauthorized("User isn't a member of the project or doesn't have rights");
            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(Participation participationUpdate)
        {
            int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier));
            var user = await _context.Participations.FirstOrDefaultAsync(p => p.UserId == UserId && p.RoleId == 1 && p.ProjectId == participationUpdate.ProjectId);
            if (user is null) return Unauthorized("User isn't a member of the project or doesn't have rights");
            var participation = await _context.Participations.FirstOrDefaultAsync(p => p.Id == participationUpdate.Id);
            if (participation is null) return NotFound();
            participation.RoleId = participationUpdate.RoleId;
            await _context.SaveChangesAsync();
            return Ok(participation);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Participation participationDelete)
        {
            int UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier));
            var user = await _context.Participations.FirstOrDefaultAsync(p => p.UserId == UserId && p.RoleId == 1 && p.ProjectId == participationDelete.ProjectId);
            if (user is null) return Unauthorized("User isn't a member of the project or doesn't have rights");
            var participation = await _context.Participations.FirstOrDefaultAsync(p => p.Id == participationDelete.Id);
            if (participation is null) return NotFound();
            _context.Participations.Remove(participation);
            int count = await _context.SaveChangesAsync();
            return Ok($"Deleted users: {count}");
        }
    }
}
