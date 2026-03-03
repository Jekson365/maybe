using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TappApi.Interfaces;
using TappApi.Models;

namespace TappApi.Controllers {
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase {
        private readonly ITaskInterface _taskRepo;

        public TaskController(ITaskInterface taskRepo) {
            _taskRepo = taskRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll() {
            var tasks = await _taskRepo.GetAllAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id) {
            var task = await _taskRepo.GetByIdAsync(id);
            if (task == null)
                throw new KeyNotFoundException("Task not found");
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem task) {
            var created = await _taskRepo.CreateAsync(task);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TaskItem task) {
            if (id != task.Id)
                return BadRequest("ID mismatch");

            var existing = await _taskRepo.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Title = task.Title;
            existing.Description = task.Description;
            existing.CreatedBy = task.CreatedBy;

            await _taskRepo.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id) {
            var deleted = await _taskRepo.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
