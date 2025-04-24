using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Context;
using PMSBackend.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PMSBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        ApplicationContext _context;
        public TasksController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int projectId)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var project = await _context.Projects.Include(p => p.Participations).Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == projectId);
            if (project is null) return NotFound();
            if (project.Participations.FirstOrDefault(par => par.UserId == userId) is null && project.Private is true) return Unauthorized();
            var tasks = project.Tasks.ToList();
            return Ok(tasks);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Models.Task task)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var project = await _context.Projects.Include(p => p.Participations) .FirstOrDefaultAsync(p => p.Id == task.ProjectId);
            if(project is null) return NotFound();
            var user = project.Participations.FirstOrDefault(p => p.UserId == userId && p.RoleId == 1);
            if (user is null) Unauthorized();
            project.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return Ok(task);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Models.Task taskUpdate)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var project = await _context.Projects.Include(p => p.Participations).FirstOrDefaultAsync(p => p.Id == taskUpdate.ProjectId);
            if (project is null) return NotFound();
            var user = project.Participations.FirstOrDefault(p => p.UserId == userId && p.RoleId == 1);
            if (user is null) Unauthorized();
            var task = project.Tasks.FirstOrDefault(t => t.Id == taskUpdate.Id);
            if(task is null) return NotFound();
            task.Status = taskUpdate.Status;
            task.Description = taskUpdate.Description;
            task.Name = taskUpdate.Name;
            await _context.SaveChangesAsync();   
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Models.Task taskDelete)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var project = await _context.Projects.Include(p => p.Participations).FirstOrDefaultAsync(p => p.Id == taskDelete.ProjectId);
            if (project is null) return NotFound();
            var user = project.Participations.FirstOrDefault(p => p.UserId == userId && p.RoleId == 1);
            if (user is null) Unauthorized();
            var task = project.Tasks.FirstOrDefault(t => t.Id == taskDelete.Id);
            if (task is null) return NotFound();
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
