using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guide.Models
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Question { get; set; }
        public string Answer { get; set; }
        public int? PostId { get; set; }
        public bool Active { get; set; } = true;
        public virtual Post Post { get; set; }
        public List<string> Links { get; set; }
    }
}