using HelsiTestTask.BL.Interfaces;
using HelsiTestTask.Domain.Entities;
using HelsiTestTask.Domain.Models;
using HelsiTestTask.Domain.Requests;
using HelsiTestTask.Domain.Responses;
using HelsiTestTask.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace HelsiTestTasks.UnitTests.Controllers
{
    [TestClass]
    public class TaskListsControllerTests
    {
        private TaskListsController _controller;
        private ITaskListService _taskListService;

        [TestInitialize]
        public void Setup()
        {
            _taskListService = Substitute.For<ITaskListService>();
            _controller = new TaskListsController(_taskListService);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldReturnOk_WhenTaskListIsValid()
        {
            // Arrange
            var request = new SaveTaskListRequest { Name = "List" };
            var userId = "user123";
            var createdTaskList = new TaskList { Id = "task123", Name = "List", OwnerId = userId };

            _taskListService.CreateAsync(request, userId).Returns(createdTaskList);

            // Act
            var result = await _controller.CreateAsync(request, userId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(createdTaskList, result.Value);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldReturnNoContent_WhenTaskListIsValid()
        {
            // Arrange
            var id = "task123";
            var request = new SaveTaskListRequest { Name = "List" };
            var userId = "user123";

            _taskListService.UpdateAsync(id, request, userId).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateAsync(id, request, userId) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldReturnNoContent_WhenTaskListIsValid()
        {
            // Arrange
            var id = "task123";
            var userId = "user123";

            _taskListService.DeleteAsync(id, userId).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteAsync(id, userId) as NoContentResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public async Task GetPagedAsync_ShouldReturnOk_WhenUserIdIsValid()
        {
            // Arrange
            var request = new GetPagedTaskListsRequest { Page = 1, PageSize = 10 };
            var userId = "user123";
            var response = new GetPagedTaskListsResponse
            {
                TaskLists = new List<TaskListResponse>
                {
                    new TaskListResponse { Id = "task123", Name = "List"}
                },
                TotalCount = 1
            };

            _taskListService.GetPagedAsync(request, userId).Returns(response);

            // Act
            var result = await _controller.GetPagedAsync(request, userId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(response, result.Value);
        }
    }
}
