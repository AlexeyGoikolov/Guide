using System.ComponentModel.DataAnnotations;

namespace Guide.ViewModels
{
    public class GlossaryViewModel
    {
        public int Id { get; set; }
        public int InterpretationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Source { get; set; }
        public string Action { get; set; }
    }
}