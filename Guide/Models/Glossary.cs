using System;
using System.ComponentModel.DataAnnotations;

namespace Guide.Models
{
    public class Glossary
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Поля не заполнено")]
        [MaxLength(25,ErrorMessage = "Максимальная длина названия 25 символов")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Поля не заполнено")]
        public string Description { get; set; }
        public string Abbreviation { get; set; }
        public string Source { get; set; }
        public bool Active { get; set; } = true;
    }
}