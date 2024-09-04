using HelsiTestTask.DAL.Interfaces;
using HelsiTestTask.Domain.Constants;
using HelsiTestTask.Domain.Entities;
using MongoDB.Driver;

namespace HelsiTestTask.DAL.Repositories
{
    public class TaskListRepository : ITaskListRepository
    {
        private readonly IMongoCollection<TaskListEntity> _taskLists;

        public TaskListRepository(IMongoDatabase database)
        {
            _taskLists = database.GetCollection<TaskListEntity>(MongoCollectionNames.TaskLists);
        }

        public async Task CreateAsync(TaskListEntity taskList)
        {
            await _taskLists.InsertOneAsync(taskList);
        }
        public async Task<int> GetCountAsync(string userId)
        {
            var filter = Builders<TaskListEntity>.Filter.Or(
                Builders<TaskListEntity>.Filter.Eq(t => t.OwnerId, userId),
                Builders<TaskListEntity>.Filter.AnyEq(t => t.SharedWith, userId)
            );

            return (int)await _taskLists.CountDocumentsAsync(filter);
        }

        public async Task<TaskListEntity> GetByIdAsync(string id)
        {
            var filter = Builders<TaskListEntity>.Filter.Eq(t => t.Id, id);

            return await _taskLists.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TaskListEntity>> GetPagedAsync(string userId, int page, int pageSize)
        {
            var filter = Builders<TaskListEntity>.Filter.Or(
                Builders<TaskListEntity>.Filter.Eq(t => t.OwnerId, userId),
                Builders<TaskListEntity>.Filter.AnyEq(t => t.SharedWith, userId)
            );

            return await _taskLists.Find(filter)
                .SortByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task UpdateAsync(string id, TaskListEntity updatedTaskList)
        {
            var filter = Builders<TaskListEntity>.Filter.Eq(t => t.Id, id);

            await _taskLists.ReplaceOneAsync(filter, updatedTaskList);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<TaskListEntity>.Filter.Eq(t => t.Id, id);

            await _taskLists.DeleteOneAsync(filter);
        }
    }
}