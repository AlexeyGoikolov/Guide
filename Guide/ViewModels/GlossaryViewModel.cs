using System.ComponentModel.DataAnnotations;
using Guide.Models;

namespace Guide.ViewModels
{
    public enum Language
    {
        ru,
        en
    }
    public class GlossaryViewModel
    {
        public int Id { get; set; }
        public int InterpretationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Source { get; set; }
        public int? GlossarysId { get; set; }
        public virtual Glossary Glossary{ get; set; }
        public Language Language { get; set; } = Language.ru;
        public string Action { get; set; }
    }
}