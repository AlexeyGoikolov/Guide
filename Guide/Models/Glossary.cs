using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guide.Models
{
    public class Glossary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Interpretation> Interpretations { get; set; }
        public bool Active { get; set; } = true;
    }
}