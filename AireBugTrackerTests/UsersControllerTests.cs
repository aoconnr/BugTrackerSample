using AireBugTracker.Controller;
using AireBugTracker.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AireBugTrackerTests
{
    public class UsersControllerTests
    {
        private UsersController _controller;

        public UsersControllerTests()
        {
            var dbContext = ContextSetup.GenerateMockContext();
            var bugService = new BugService(dbContext);
            var userService = new UserService(bugService, dbContext);

            _controller = new UsersController(userService);
        }

        [Fact]
        public async Task GetAll()
        {
            var response = await _controller.Get();

            Assert.NotNull(response);
            Assert.Equal(3, response.Count);
        }

        [Fact]
        public async Task GetById()
        {
            var response = await _controller.Get(1);
            Assert.Equal("Andrew", response.Name);
        }

        [Fact]
        public async Task Create()
        {
            var userName = "New user";
            var response = await _controller.Create(userName);

            Assert.Equal(userName, response.Name);
        }

        [Fact]
        public async Task Delete()
        {
            await _controller.Delete(1);
            try
            {
                var response = await _controller.Get(1);
            }
            catch (Exception ex)
            {
                Assert.IsType<Exception>(ex);
            }
        }
    }
}