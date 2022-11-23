using AireBugTracker.Data;
using AireBugTracker.Shared;
using Microsoft.AspNetCore.Mvc;

namespace AireBugTracker.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _UserService;

        public UsersController(IUserService UserService)
        {
            _UserService = UserService;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _UserService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<User> Get(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));
            return await _UserService.Get(id);
        }

        public async Task Rename(int id, [FromBody] string name)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var user = await _UserService.Get(id);
            if (user is null) throw new ArgumentNullException();

            user.Name = name;
            await _UserService.Update(user);
        }

        [HttpPut()]
        public async Task<User> Create([FromBody] String name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var user = new User { Name = name };
            await _UserService.Create(user);
            return user;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));
            await _UserService.Delete(id);
        }
    }
}
