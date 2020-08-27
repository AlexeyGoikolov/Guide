using System.ComponentModel.DataAnnotations;

namespace Guide.ViewModels
{
    public class GlossaryViewModel
    {
        public int Id { get; set; }
        public int InterpretationId { get; set; }

        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Source { get; set; }
        public string Action { get; set; }
    }
}