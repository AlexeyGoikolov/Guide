#nullable enable
using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class ListUsersViwModel
    {
        public string? Action { get; set; }
        public string? idPosition { get; set; }
        public List<User> Users { get; set; }
        public List<Position> Positions { get; set; }
    }
}