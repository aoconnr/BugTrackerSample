using AireBugTracker.Exceptions;
using AireBugTracker.Shared;
using Microsoft.EntityFrameworkCore;

namespace AireBugTracker.Data
{
    public interface IBugService
    {
        Task Create(Bug bug);
        Task Update(Bug bug);
        Task Delete(int id);
        Task<Bug> Get(int id);
        Task<List<Bug>> GetAll(bool incClosed = false);
        Task<List<Bug>> GetByAssignedUser(int userId);
        Task Close(Bug bug);
    }

    public class BugService : IBugService
    {
        readonly DatabaseContext _dbContext = new DatabaseContext();

        public BugService(DatabaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task Create(Bug bug)
        {
            if (bug is null) throw new ArgumentNullException(nameof(bug));

            bug.OpenedDate = DateTime.Now;
            await _dbContext.AddAsync(bug);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Bug bug)
        {
            if (bug is null) throw new ArgumentNullException(nameof(bug));

            _dbContext.Bugs.Update(bug);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var bug = _dbContext.Bugs.Find(id);
            if (bug != null)
            {
                _dbContext.Bugs.Remove(bug);
                await _dbContext.SaveChangesAsync();
            }
        }

        public Task<Bug> Get(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));

            var bug = _dbContext.Bugs.Find(id);

            if (bug is null) throw new ObjectNotFoundException(nameof(bug));

            _dbContext.Entry(bug).State = EntityState.Detached;

            return Task.FromResult(bug);
        }

        public Task<List<Bug>> GetAll(bool incClosed = false)
        {
            return Task.FromResult(incClosed
                ? _dbContext.Bugs.ToList()
                : _dbContext.Bugs.Where(b => b.ClosedDate == null).ToList()
            );
        }

        public Task<List<Bug>> GetByAssignedUser(int userId)
        {
            return Task.FromResult(_dbContext.Bugs.Where(b => b.AssignedUser != null && b.AssignedUser.Id == userId).ToList());
        }

        public async Task Close(Bug bug)
        {
            if (bug is null) throw new ArgumentNullException(nameof(bug));

            bug.ClosedDate = DateTime.Now;
            _dbContext.Bugs.Update(bug);
            await _dbContext.SaveChangesAsync();
        }

    }
}
