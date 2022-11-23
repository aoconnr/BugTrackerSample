using AireBugTracker.Controller;
using AireBugTracker.Data;
using AireBugTracker.Exceptions;

namespace AireBugTrackerTests
{
    public class BugsControllerTests
    {
        private BugsController _controller;
        private UserService _userService;
        private BugService _bugService;

        public BugsControllerTests()
        {
            var dbContext = ContextSetup.GenerateMockContext();
            _bugService = new BugService(dbContext);
            _userService = new UserService(_bugService, dbContext);

            _controller = new BugsController(_bugService, _userService);
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
            Assert.Equal("Bug 1", response.Title);
        }

        [Fact]
        public async Task GetByIdInvalid()
        {
            try
            {
                var response = await _controller.Get(0);
                Assert.True(false, "Expected Error not thrown");
            }
            catch(Exception ex)
            {
                Assert.IsType<ArgumentNullException>(ex);
            }
        }

        [Fact]
        public async Task CreateWithoutUser()
        {
            var request = new CreateBugRequest
            {
                Title = "Test Title",
                Description = "Test Description"
            };
            var response = await _controller.Create(request);

            Assert.Equal(request.Title, response.Title);
            Assert.Equal(request.Description, response.Description);
        }

        [Fact]
        public async Task CreateWithUser()
        {
            var request = new CreateBugRequest
            {
                Title = "Test Title",
                Description = "Test Description",
                AssignedUserId = 1
            };
            var response = await _controller.Create(request);

            Assert.Equal(request.Title, response.Title);
            Assert.Equal(request.Description, response.Description);
            Assert.Equal(request.AssignedUserId, response.AssignedUser.Id);
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
                Assert.IsType<ObjectNotFoundException>(ex);
            }
        }

        [Fact]
        public async Task AssignUser()
        {
            var bug = await _bugService.Get(1);
            Assert.Null(bug.AssignedUser);

            await _controller.AssignUser(1, 1);
            bug = await _controller.Get(1);

            Assert.NotNull(bug.AssignedUser);
            Assert.Equal(1, bug.AssignedUser.Id);
        }

        [Fact]
        public async Task AssignUserInvalidUserId()
        {
            try
            {
                await _controller.AssignUser(1, 10);
                Assert.True(false, "Expected Error not thrown");
            }
            catch (Exception ex)
            {
                Assert.IsType<ObjectNotFoundException>(ex);
            }
        }

        [Fact]
        public async Task AssingUserInvalidBugId()
        {
            try
            {
                await _controller.AssignUser(10, 1);
                Assert.True(false, "Expected Error not thrown");
            }
            catch (Exception ex)
            {
                Assert.IsType<ObjectNotFoundException>(ex);
            }
        }
    }
}