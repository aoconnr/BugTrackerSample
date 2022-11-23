using AireBugTracker.Exceptions;
using AireBugTracker.Shared;

namespace AireBugTracker.Data
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User> Get(int id);
        Task Create(User user);
        Task Update(User user);
        Task Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly IBugService _bugService;
        private readonly DatabaseContext _dbContext = new DatabaseContext();

        public UserService(IBugService bugService, DatabaseContext dbContext)
        {
            _bugService = bugService ?? throw new ArgumentNullException(nameof(bugService));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Create(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));

            var bugs = await _bugService.GetByAssignedUser(id);
            if (bugs.Any()) throw new UserAssignedToBugsException();

            var user = _dbContext.Users.Find(id);
            if (user is null) throw new ObjectNotFoundException(nameof(user));

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();

        }

        public Task<User> Get(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));

            var user = _dbContext.Users.Find(id);
            if (user is null) throw new ObjectNotFoundException(nameof(user));

            //_dbContext.Entry(user).State = EntityState.Detached;

            return Task.FromResult(user);
        }

        public Task<List<User>> GetAll()
            => Task.FromResult(_dbContext.Users.ToList());

    }
}
