using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMSBackend.Context;
using PMSBackend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PMSBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        ApplicationContext _context;
        public ValuesController(ApplicationContext context)
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
                return BadRequest();
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
            return NoContent();
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
            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId);
            if (project is null)
                return NotFound($"Project with id={projectId} not found");
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
