using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Context;
using PMSBackend.Models;
using System.Security.Claims;

namespace PMSBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        ApplicationContext _context;
        public StatusesController(ApplicationContext context) 
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int projectId)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _context.Projects.Include(p => p.Participations).FirstOrDefaultAsync(p => p.Private == false ||
                p.Participations.FirstOrDefault(par => par.UserId == userId) != null);
            if (user is null) return Unauthorized();
            var statuses = await _context.Statuses.Where(s => s.ProjectId == projectId).ToListAsync();
            return statuses is null ? NotFound() : Ok(statuses);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Status status)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _context.Participations.FirstOrDefaultAsync(p => p.ProjectId == status.ProjectId && p.UserId == userId && p.RoleId == 1);
            if (user is null) return Unauthorized();
            await _context.Statuses.AddAsync(status);
            await _context.SaveChangesAsync();
            return Ok(status);    
        }
        [HttpPut]
        public async Task<IActionResult> Update(Status statusUpdate)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _context.Participations.FirstOrDefaultAsync(p => p.ProjectId == statusUpdate.ProjectId && p.UserId == userId && p.RoleId == 1);
            if (user is null) return Unauthorized();
            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == statusUpdate.Id);
            if (status is null) return NotFound();
            status.Name = statusUpdate.Name;
            await _context.SaveChangesAsync();
            return Ok(status);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Status statusDelete)
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await _context.Participations.FirstOrDefaultAsync(p => p.ProjectId == statusDelete.ProjectId && p.UserId == userId && p.RoleId == 1);
            if(user is null) return Unauthorized();
            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Id == statusDelete.Id);
            if(status is null) return NotFound();
            _context.Remove(status);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
