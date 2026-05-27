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
    }
}
