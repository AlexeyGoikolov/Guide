using System;
using Microsoft.AspNetCore.Identity;

namespace Guide.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? PositionId { get; set; }
        public virtual Position Position { get; set; }
        
        public bool Active { get; set; } = true;
        
        public DateTime DateCreate { get; set; } = DateTime.Now;

    }
}