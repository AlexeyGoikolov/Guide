using System;
using Guide.Models;
using Type = Guide.Models.Type;


namespace Guide.ViewModels
{
    public class LibraryListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime DateCreate { get; set; }

        public string FilePath { get; set; }
        
        // Тип котнетна
        public virtual TypeContent TypeContent { get; set; }
        //Категория
        public virtual Category Category { get; set; }
        //тип файла
        public virtual Type Type { get; set; }
        //Состояние
        public virtual TypeState TypeState { get; set; }
        public bool Active { get; set; }
        
    }
}