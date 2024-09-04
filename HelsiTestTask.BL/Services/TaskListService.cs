using HelsiTestTask.BL.Interfaces;
using HelsiTestTask.BL.Mappers;
using HelsiTestTask.DAL.Interfaces;
using HelsiTestTask.Domain.Entities;
using HelsiTestTask.Domain.Models;
using HelsiTestTask.Domain.Requests;
using HelsiTestTask.Domain.Responses;
using MongoDB.Driver;

namespace HelsiTestTask.BL.Services
{
    public class TaskListService : ITaskListService
    {
        private readonly ITaskListRepository _repository;

        public TaskListService(ITaskListRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskList> CreateAsync(SaveTaskListRequest request, string userId)
        {
            var entity = request.ToEntity(userId);

            await _repository.CreateAsync(entity);

            return entity.ToModel();
        }

        public async Task UpdateAsync(string id, SaveTaskListRequest request, string userId)
        {
            var existingEntity = await _repository.GetByIdAsync(id);

            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Task list with id '{id}' not found.");
            }

            if (existingEntity.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("Only the owner can update the task list.");
            }

            var updatedEntity = request.ToEntity(userId);
            updatedEntity.Id = id; 

            await _repository.UpdateAsync(id, updatedEntity);
        }

        public async Task DeleteAsync(string id, string userId)
        {
            var taskList = await _repository.GetByIdAsync(id);

            if (taskList == null)
            {
                throw new KeyNotFoundException("Task list not found.");
            }

            if (taskList.OwnerId != userId)
            {
                throw new UnauthorizedAccessException("Only the owner can delete this task list.");
            }

            await _repository.DeleteAsync(id);
        }

        public async Task<TaskListEntity> GetByIdAsync(string id, string userId)
        {
            var taskList = await _repository.GetByIdAsync(id);

            if (taskList == null)
            {
                throw new KeyNotFoundException("Task list not found.");
            }

            if (taskList.OwnerId != userId && !taskList.SharedWith.Contains(userId))
            {
                throw new UnauthorizedAccessException("You do not have permission to view this task list.");
            }

            return taskList;
        }

        public async Task<GetPagedTaskListsResponse> GetPagedAsync(GetPagedTaskListsRequest request, string userId)
        {
            if (request.Page <= 0) throw new ArgumentException("Page must be greater than 0.", nameof(request.Page));

            if (request.PageSize <= 0) throw new ArgumentException("PageSize must be greater than 0.", nameof(request.PageSize));

            if (string.IsNullOrEmpty(userId)) throw new ArgumentException("UserId cannot be null or empty.", nameof(userId));

            var totalCount = await _repository.GetCountByUserAsync(userId);

            var taskLists = await _repository.GetPagedAsync(userId, request.Page, request.PageSize);

            return new GetPagedTaskListsResponse
            {
                TaskLists = taskLists.Select(x => x.ToResponse()),
                TotalCount = totalCount
            };
        }
    }
}