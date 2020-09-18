using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guide.Models
{
    public enum Language
    {
        ru,
        en
    }
    public class Glossary
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Interpretation> Interpretations { get; set; }
        public int? GlossarysId { get; set; }//русский язык хранит ID английского
        public virtual Glossary Glossarys{ get; set; }
        public Language Language { get; set; } = Language.ru;

        public bool Active { get; set; } = true;
        [NotMapped]
        public virtual List<Interpretation> GlossarysInterpretations { get; set; }
        
    }
}