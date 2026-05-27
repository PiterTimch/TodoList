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
        public async Task<IActionResult> Get()
        {
            var response = await taskService.SearchTasksAsync(new TasksSearchRequestModel());
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskRequestModel model)
        {
            try
            {
                var result = await taskService.CreateTaskAsync(model);
                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
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
    }
}
