using Microsoft.AspNetCore.Identity;

namespace Guide.Models
{
    public class User : IdentityUser
    {
        public bool Active { get; set; } = true;
    }
}