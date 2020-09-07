using System.Collections.Generic;
using System.Linq;
using Guide.Models;
using Guide.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace Guide.Services
{
    public class UserRepository : IUserRepository
    {
        private GuideContext _db;

        public UserRepository(GuideContext db)
        {
            _db = db;
        }

        public UserRepository()
        {
        }

        public virtual IEnumerable<User> GetAllUsers() => _db.Users;


        public virtual User GetUser(string id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        public virtual void Update(User user) => _db.Entry(user).State = EntityState.Modified;

        public virtual void Save() => _db.SaveChanges();

        public virtual async void SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        public virtual TaskUser GetUserTask(string userId)
        {
            return _db.TaskUsers.FirstOrDefault(t => t.UserId == userId);
        }

        public virtual List<Issue> GetUserIssues(string userId)
        {
            return _db.UserIssues.OrderBy(d => d.Id)
                .Where(d => d.UserId == userId).
                Select(s => s.Issue).ToList();
        }
        
        public virtual void AddPosition(Position position) => _db.Positions.Add(position);

        public virtual Position GetPosition(int positionId) => _db.Positions.FirstOrDefault(p => p.Id == positionId);
        
        public virtual List<Position> GetAllPositions() => _db.Positions.ToList();
        public virtual List<Position> GetActivePositions()
        {
            return _db.Positions.Where(p => p.Active).ToList();
        }
    }
}