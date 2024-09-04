using HelsiTestTask.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/task-lists/{taskListId}/sharing")]
public class TaskListSharingController : ControllerBase
{
    private readonly ITaskListSharingService _taskListSharingService;

    public TaskListSharingController(ITaskListSharingService taskListSharingService)
    {
        _taskListSharingService = taskListSharingService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Shares the task list with another user", Description = "Allows the specified user to access the task list.")]
    [SwaggerResponse(204, "Task list shared successfully")]
    [SwaggerResponse(400, "Task list ID, user ID, or target user ID is invalid.")]
    public async Task<IActionResult> ShareWithUserAsync(
        string taskListId,
        [FromHeader(Name = "UserId")] string userId,
        [FromQuery] string targetUserId)
    {
        if (string.IsNullOrEmpty(taskListId) || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(targetUserId))
        {
            return BadRequest("Task list ID, user ID, or target user ID is invalid.");
        }

        await _taskListSharingService.ShareWithUserAsync(taskListId, userId, targetUserId);

        return NoContent();
    }

    [HttpDelete]
    [SwaggerOperation(Summary = "Removes a user's access to the task list", Description = "Revokes the specified user's access to the task list.")]
    [SwaggerResponse(204, "User access removed successfully")]
    [SwaggerResponse(400, "Task list ID, user ID, or target user ID is invalid.")]
    public async Task<IActionResult> RemoveConnectionAsync(
        string taskListId,
        [FromHeader(Name = "UserId")] string userId,
        [FromQuery] string targetUserId)
    {
        if (string.IsNullOrEmpty(taskListId) || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(targetUserId))
        {
            return BadRequest("Task list ID, user ID, or target user ID is invalid.");
        }

        await _taskListSharingService.RemoveUserSharingAsync(taskListId, userId, targetUserId);

        return NoContent();
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Retrieves the list of users with whom the task list is shared", Description = "Returns a list of user IDs who have access to the task list.")]
    [SwaggerResponse(200, "Shared users retrieved successfully", typeof(List<string>))]
    [SwaggerResponse(400, "Task list ID or user ID is invalid.")]
    public async Task<IActionResult> GetSharedUsersAsync(
        string taskListId,
        [FromHeader(Name = "UserId")] string userId)
    {
        if (string.IsNullOrEmpty(taskListId) || string.IsNullOrEmpty(userId))
        {
            return BadRequest("Task list ID or user ID is invalid.");
        }

        var sharedUsers = await _taskListSharingService.GetSharedUsersAsync(taskListId, userId);

        return Ok(sharedUsers);
    }
}