using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PMSBackend.Context;
using PMSBackend.Models;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PMSBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        ApplicationContext _context;

        public ProjectsController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var projects = await _context.Projects.ToListAsync();
            return projects is null ? NotFound() : Ok(projects);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Project project)
        {
            if (project is null)
                return BadRequest("badRequest");
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            await _context.Participations.AddAsync(new Participation
            {
                RoleId = 1,
                UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                ProjectId = project.Id
            });
            await _context.Statuses.AddRangeAsync(new List<Status>()
            {
                new Status { Name = "Новые", ProjectId = project.Id },
                new Status { Name = "В работе", ProjectId = project.Id },
                new Status{Name= "Можно проверять", ProjectId = project.Id},
                new Status { Name = "Готово", ProjectId = project.Id }
            });
            await _context.SaveChangesAsync();
            return Ok(project.Id);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Project projectUpdate)
        {
            if(projectUpdate is null)
                return BadRequest();
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectUpdate.Id);
            if (project is null)
                return NotFound($"Project with id={projectUpdate.Id} not found");
            project.Private = projectUpdate.Private;
            project.Description = projectUpdate.Description;
            project.Name = projectUpdate.Name;
            await _context.SaveChangesAsync();
            return Ok(project);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int projectId)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId && p.Participations.FirstOrDefault(par => par.UserId == Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value) && par.RoleId == 1) != default);
            if (project is null)
                return NotFound($"Project with id={projectId} not found or user don't have access");
            _context.Projects.Remove(project);
            return NoContent();
        }
    }
}
