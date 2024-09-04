using HelsiTestTask.BL.Services;
using HelsiTestTask.DAL.Interfaces;
using HelsiTestTask.Domain.Entities;
using HelsiTestTask.Domain.Requests;
using HelsiTestTask.BL.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using NSubstitute;
using FluentAssertions;

namespace HelsiTestTasks.UnitTests.Services
{
    [TestClass]
    public class TaskListServiceTests
    {
        private ITaskListRepository _repository;
        private TaskListService _service;

        [TestInitialize]
        public void Setup()
        {
            _repository = Substitute.For<ITaskListRepository>();
            _service = new TaskListService(_repository);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateTaskList()
        {
            // Arrange
            var request = new SaveTaskListRequest { Name = "List", Tasks = new List<string> { "1", "2" } };
            var userId = "user1";
            var entity = request.ToEntity(userId);

            _repository.CreateAsync(Arg.Any<TaskListEntity>()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(request, userId);

            // Assert
            await _repository.Received(1).CreateAsync(Arg.Is<TaskListEntity>(e =>
                e.Name == entity.Name &&
                e.OwnerId == entity.OwnerId
            ));

            result.Name.Should().Be(entity.Name);
            result.OwnerId.Should().Be(entity.OwnerId);
            result.Tasks.Should().BeEquivalentTo(entity.Tasks);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateTaskList()
        {
            // Arrange
            var id = "1";
            var request = new SaveTaskListRequest { Name = "List" };
            var userId = "user1";
            var existingEntity = new TaskListEntity { Id = id, OwnerId = userId };
            var updatedEntity = request.ToEntity(userId);
            updatedEntity.Id = id;

            _repository.GetByIdAsync(id).Returns(Task.FromResult(existingEntity));
            _repository.UpdateAsync(id, Arg.Any<TaskListEntity>()).Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(id, request, userId);

            // Assert
            await _repository.Received(1).UpdateAsync(id, Arg.Any<TaskListEntity>());
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteTaskList()
        {
            // Arrange
            var id = "1";
            var userId = "user1";
            var existingEntity = new TaskListEntity { Id = id, OwnerId = userId };

            _repository.GetByIdAsync(id).Returns(Task.FromResult(existingEntity));
            _repository.DeleteAsync(id).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(id, userId);

            // Assert
            await _repository.Received(1).DeleteAsync(id);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnTaskList()
        {
            // Arrange
            var id = "1";
            var userId = "user1";
            var taskList = new TaskListEntity { Id = id, OwnerId = userId, SharedWith = new List<string>() };

            _repository.GetByIdAsync(id).Returns(Task.FromResult(taskList));

            // Act
            var result = await _service.GetByIdAsync(id, userId);

            // Assert
            Assert.AreEqual(taskList, result);
        }

        [TestMethod]
        public async Task GetPagedAsync_ShouldReturnPagedTaskLists()
        {
            // Arrange
            var userId = "user1";
            var request = new GetPagedTaskListsRequest { Page = 1, PageSize = 10 };
            var taskLists = new List<TaskListEntity>
            {
                new TaskListEntity { Name = "List1", OwnerId = userId },
                new TaskListEntity { Name = "List2", OwnerId = userId }
            };

            _repository.GetPagedAsync(userId, request.Page, request.PageSize)
                .Returns(Task.FromResult((IEnumerable<TaskListEntity>)taskLists));

            _repository.GetCountAsync(Arg.Any<FilterDefinition<TaskListEntity>>())
                .Returns(Task.FromResult(taskLists.Count));

            // Act
            var result = await _service.GetPagedAsync(request, userId);

            // Assert
            result.TotalCount.Should().Be(taskLists.Count);
        }
    }
}
