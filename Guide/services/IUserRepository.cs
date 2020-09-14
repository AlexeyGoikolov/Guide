using System.Collections.Generic;
using Guide.Models;

namespace Guide.Services
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(string id);
        void Update(User user);
        void Save();

        void SaveAsync();
        TaskUser GetUserTask(string userId);
        List<Issue> GetUserIssues(string userId);

        void AddPosition(Position position);

        List<Issue> PositionsIssues(int positionId);

        Position GetPosition(int positionId);
        List<Position> GetAllPositions();
        List<Position> GetActivePositions();
    }
}