using HelsiTestTask.Domain.Entities;
using MongoDB.Driver;

namespace HelsiTestTask.DAL.Interfaces
{
    public interface ITaskListRepository
    {
        Task CreateAsync(TaskListEntity taskList);

        Task<TaskListEntity> GetByIdAsync(string id);

        Task<IEnumerable<TaskListEntity>> GetPagedAsync(string userId, int page, int pageSize);

        Task UpdateAsync(string id, TaskListEntity updatedTaskList);

        Task DeleteAsync(string id);

        Task<int> GetCountAsync(FilterDefinition<TaskListEntity> filter);
    }
}
