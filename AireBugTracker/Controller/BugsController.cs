using AireBugTracker.Data;
using AireBugTracker.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AireBugTracker.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly IBugService _bugService;
        private readonly IUserService _userService;

        public BugsController(IBugService bugService, IUserService userService)
        {
            _bugService = bugService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<List<Bug>> Get([FromQuery]Boolean incClosed = false)
        {
            return await _bugService.GetAll(incClosed);
        }

        [HttpGet("{id}")]
        public async Task<Bug> Get(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));
            return await _bugService.Get(id);
        }

        [HttpPost("{bugId}/users/{userId}")]
        public async Task AssignUser(int bugId, int userId)
        {
            if (bugId == 0) throw new ArgumentNullException(nameof(bugId));
            if (userId == 0) throw new ArgumentNullException(nameof(userId));

            var user = await _userService.Get(userId);
            var bug = await _bugService.Get(bugId);

            bug.AssignedUser = user;

            await _bugService.Update(bug);
        }


        [HttpPut()]
        public async Task<Bug> Create([FromBody] CreateBugRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            //create bug object with assigned user if included
            var bug = new Bug { Title = request.Title, Description = request.Description };
            if (request.AssignedUserId > 0) bug.AssignedUser = await _userService.Get(request.AssignedUserId);

            await _bugService.Create(bug);
            return bug;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));

            await _bugService.Delete(id);
        }
    }
}
