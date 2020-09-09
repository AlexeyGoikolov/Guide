using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;

namespace Guide.Models
{
    public class Interpretation
    {
        public int Id { get; set; }
        public int GlossaryId { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Source { get; set; }
        public bool Active { get; set; } = true;
    }
}