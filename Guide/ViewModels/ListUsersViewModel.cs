#nullable enable
using System.Collections.Generic;
using Guide.Models;

namespace Guide.ViewModels
{
    public class ListUsersViewModel
    {
        public string? Action { get; set; }
        public int? IdPosition { get; set; }
        public List<User> Users { get; set; }
        public List<Position> Positions { get; set; }
    }
}