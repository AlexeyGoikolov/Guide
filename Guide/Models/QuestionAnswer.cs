using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guide.Models
{
    public class QuestionAnswer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required(ErrorMessage = "Поля не заполнено")]
        public string Question { get; set; }
        public string Answer { get; set; }
        public List<string> Links { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}