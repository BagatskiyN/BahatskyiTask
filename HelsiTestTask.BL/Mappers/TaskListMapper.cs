using HelsiTestTask.Domain.Entities;
using HelsiTestTask.Domain.Models;
using HelsiTestTask.Domain.Requests;
using HelsiTestTask.Domain.Responses;
using MongoDB.Bson;

namespace HelsiTestTask.BL.Mappers
{
    public static class TaskListMapper
    {
        public static TaskList ToModel(this TaskListEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new TaskList
            {
                Id = entity.Id,
                CreatedAt = entity.CreatedAt,
                Name = entity.Name,
                OwnerId = entity.OwnerId,
                Tasks = entity.Tasks,
                SharedWith = entity.SharedWith
            };
        }

        public static TaskListResponse ToResponse(this TaskListEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new TaskListResponse
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }

        public static TaskListEntity ToEntity(this SaveTaskListRequest request, string userId)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

            return new TaskListEntity
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CreatedAt = DateTime.UtcNow,
                Name = request.Name,
                OwnerId = userId,
                Tasks = request.Tasks ?? new List<string>(),
                SharedWith = request.SharedWith ?? new List<string>()
            };
        }
    }
}
