using HelsiTestTask.BL.Interfaces;
using HelsiTestTask.DAL.Interfaces;

namespace HelsiTestTask.BL.Services
{
    public class TaskListSharingService : ITaskListSharingService
    {
        private readonly ITaskListRepository _repository;

        public TaskListSharingService(ITaskListRepository repository)
        {
            _repository = repository;
        }
        public async Task ShareWithUserAsync(string id, string userId, string targetUserId)
        {
            var taskList = await _repository.GetByIdAsync(id);
            if (taskList == null)
            {
                throw new KeyNotFoundException("Task list not found.");
            }

            if (taskList.OwnerId != userId && !taskList.SharedWith.Contains(userId))
            {
                throw new UnauthorizedAccessException("You do not have permission to share this task list.");
            }

            if (taskList.SharedWith.Contains(targetUserId))
            {
                throw new ArgumentException("User is already connected to this task list.");
            }

            taskList.SharedWith.Add(targetUserId);
            await _repository.UpdateAsync(id, taskList);
        }

        public async Task RemoveUserSharingAsync(string id, string userId, string targetUserId)
        {
            var taskList = await _repository.GetByIdAsync(id);
            if (taskList == null)
            {
                throw new KeyNotFoundException("Task list not found.");
            }

            if (taskList.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("Only the owner can remove connections from this task list.");
            }

            if (!taskList.SharedWith.Contains(targetUserId))
            {
                throw new ArgumentException("User is not connected to this task list.");
            }

            taskList.SharedWith.Remove(targetUserId);

            await _repository.UpdateAsync(id, taskList);
        }

        public async Task<List<string>> GetSharedUsersAsync(string id, string userId)
        {
            var taskList = await _repository.GetByIdAsync(id);

            if (taskList == null)
            {
                throw new KeyNotFoundException("Task list not found.");
            }

            if (taskList.OwnerId != userId && !taskList.SharedWith.Contains(userId))
            {
                throw new UnauthorizedAccessException("Only the owner or a shared user can view shared users.");
            }

            return taskList.SharedWith;
        }
    }
}