using HelsiTestTask.Domain.Entities;
using HelsiTestTask.Domain.Models;
using HelsiTestTask.Domain.Requests;
using HelsiTestTask.Domain.Responses;

namespace HelsiTestTask.BL.Interfaces
{
    public interface ITaskListService
    {
        Task<TaskList> CreateAsync(SaveTaskListRequest request, string userId);

        Task UpdateAsync(string id, SaveTaskListRequest request, string userId);

        Task DeleteAsync(string id, string userId);

        Task<TaskListEntity> GetByIdAsync(string id, string userId);

        Task<GetPagedTaskListsResponse> GetPagedAsync(GetPagedTaskListsRequest request, string userId);
    }
}