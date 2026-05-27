using BLL.Interfaces;
using BLL.Models.Task;
using Microsoft.AspNetCore.Mvc;

namespace TodoListApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController(ITaskService taskService) : ControllerBase
    {

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] TasksSearchRequestModel model)
        {
            try
            {
                var response = await taskService.SearchTasksAsync(model);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute(Name = "id")] string stringId)
        {
            if (!long.TryParse(stringId, out long id))
            {
                return BadRequest(new { message = "Invalid ID format" });
            }

            try
            {
                var task = await taskService.GetTaskByIdAsync(id);
                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequestModel model)
        {
            try
            {
                var result = await taskService.CreateTaskAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute(Name = "id")] string stringId)
        {
            if (!long.TryParse(stringId, out long id))
            {
                return BadRequest(new { message = "Invalid ID format" });
            }

            try
            {
                await taskService.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("complete")]
        public async Task<IActionResult> SetCompleted([FromBody] SetTaskCompletedRequestModel model)
        {
            try
            {
                await taskService.SetTaskCompletedAsync(model);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
