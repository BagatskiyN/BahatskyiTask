using HelsiTestTask.BL.Interfaces;
using HelsiTestTask.Domain.Models;
using HelsiTestTask.Domain.Requests;
using HelsiTestTask.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HelsiTestTask.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskListsController : ControllerBase
    {
        private readonly ITaskListService _taskListService;

        public TaskListsController(ITaskListService taskListService)
        {
            _taskListService = taskListService;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new task list", Description = "Creates a new task list for the specified user.")]
        [SwaggerResponse(200, "Task list created successfully", typeof(TaskList))]
        [SwaggerResponse(400, "Task list or user ID is invalid.")]
        public async Task<IActionResult> CreateAsync([FromBody] SaveTaskListRequest taskList, [FromHeader(Name = "UserId")] string userId)
        {
            if (taskList == null || string.IsNullOrEmpty(userId))
            {
                return BadRequest("Task list or user ID is invalid.");
            }

            var createdTaskList = await _taskListService.CreateAsync(taskList, userId);

            return Ok(createdTaskList);
        }


        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates an existing task list", Description = "Updates an existing task list with the specified ID.")]
        [SwaggerResponse(204, "Task list updated successfully")]
        [SwaggerResponse(400, "Task list, ID, or user ID is invalid.")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] SaveTaskListRequest taskList, [FromHeader(Name = "UserId")] string userId)
        {
            if (taskList == null || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(userId))
            {
                return BadRequest("Task list, ID, or user ID is invalid.");
            }

            await _taskListService.UpdateAsync(id, taskList, userId);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes an existing task list", Description = "Deletes the task list with the specified ID.")]
        [SwaggerResponse(204, "Task list deleted successfully")]
        [SwaggerResponse(400, "ID or user ID is invalid.")]
        public async Task<IActionResult> DeleteAsync(string id, [FromHeader(Name = "UserId")] string userId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(userId))
            {
                return BadRequest("ID or user ID is invalid.");
            }

            await _taskListService.DeleteAsync(id, userId);

            return NoContent();
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Retrieves a task list by ID", Description = "Retrieves the task list with the specified ID.")]
        [SwaggerResponse(200, "Task list retrieved successfully", typeof(TaskList))]
        [SwaggerResponse(400, "ID or user ID is invalid.")]
        [SwaggerResponse(404, "Task list not found")]
        public async Task<IActionResult> GetByIdAsync(string id, [FromHeader(Name = "UserId")] string userId)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(userId))
            {
                return BadRequest("ID or user ID is invalid.");
            }

            var taskList = await _taskListService.GetByIdAsync(id, userId);

            if (taskList == null)
            {
                return NotFound();
            }

            return Ok(taskList);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Retrieves a paginated list of task lists", Description = "Retrieves a paginated list of task lists for the specified user.")]
        [SwaggerResponse(200, "Task lists retrieved successfully", typeof(GetPagedTaskListsResponse))]
        [SwaggerResponse(400, "User ID is required.")]
        public async Task<IActionResult> GetPagedAsync([FromQuery] GetPagedTaskListsRequest request, [FromHeader(Name = "UserId")] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var response = await _taskListService.GetPagedAsync(request, userId);

            return Ok(response);
        }
    }
}