using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VulnTracker.DataAccess.Context;
using VulnTracker.Domain.Entities;

namespace VulnTracker.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly VulnTrackerDbContext _context;

        public ProjectController(VulnTrackerDbContext context)
        {
            _context = context;
        }

        // GET: api/Project
        [HttpGet]
        public IActionResult GetAll()
        {
            var projects = _context.Projects.ToList();
            return Ok(projects);
        }

        // GET: api/Project/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var project = _context.Projects.Find(id);
            if (project == null) return NotFound();

            return Ok(project);
        }

        // POST: api/Project
        [HttpPost]
        public IActionResult Create([FromBody] Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        // PUT: api/Project/{id}
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] Project project)
        {
            var existingProject = _context.Projects.Find(id);
            if (existingProject == null) return NotFound();

            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Project/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var project = _context.Projects.Find(id);
            if (project == null) return NotFound();

            _context.Projects.Remove(project);
            _context.SaveChanges();
            return NoContent();
        }
    }
}

