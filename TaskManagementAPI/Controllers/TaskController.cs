using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly ApplicationDBContext _context;
        public TaskController(ApplicationDBContext context) {
            _context = context;
        }

        //GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<List<TaskModel>>> GetTasks() {
            return await _context.Tasks.ToListAsync();
        }

        //GET: api/Task/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskModel>> GetTask(int id) { 
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) { 
                return NotFound();
            }

            return task;
        }

        //POST:  api/Task
        [HttpPost]
        public async Task<ActionResult<TaskModel>> SaveTask(TaskModel task) { 
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        //PUT: api/Task/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, TaskModel task) {
            if (id != task.Id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch
            {
                if (!_context.Tasks.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        //DELETE api/Task/5
        [HttpDelete]
        public async Task<ActionResult> DeleteTask(int id) {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
