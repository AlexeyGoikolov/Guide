using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace Guide.Models
{
    public class Source
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string SourceDescription { get; set; }
        public string CoverPath { get; set; }
        public string Edition { get; set; }
        public string VirtualPath { get; set; }
        public string PhysicalPath { get; set; } = null;
        public bool Active { get; set; } = true;
        public bool IsRecipe { get; set; } = false;
        public int? SourceTypeId { get; set; }
        public virtual SourceType SourceType { get; set; }

        public int? SourceStateId { get; set; }
        public virtual SourceState SourceState { get; set; }
        public string YearOfWriting { get; set; }
        public DateTime DateCreate { get; set; } = DateTimeOffset.Now.UtcDateTime.AddHours(6);
        public DateTime DateUpdate { get; set; } = DateTimeOffset.Now.UtcDateTime.AddHours(6);
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int TranslationID { get; set; }
        public string Keys { get; set; }
        public string AdditionalInformation { get; set; }
        [NotMapped]
        public virtual List<Author> Authors { get; set; }
        [NotMapped]
        public virtual List<BusinessProcess> BusinessProcesses { get; set; }
        
    }
}