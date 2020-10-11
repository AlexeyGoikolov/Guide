using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guide.Models
{
    public enum State
    {
        Новый,
        Ответили
    }
    public class QuestionAnswer
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Question { get; set; }
        public string Answer { get; set; }
        public State State { get; set; } = State.Новый;
        // тот ,кто задал вопрос
        public string AskingId{ get; set; }
        public virtual User Asking{ get; set; }
        // тот ,кто ответил на вопрос
        public string ResponderId{ get; set; }
        public virtual User Responder{ get; set; }
        public int? SourceId { get; set; }
        public bool Active { get; set; } = true;
        // к какому материалу задан вопрос
        public virtual Source Source { get; set; }
        public List<string> Links { get; set; }
    }
}