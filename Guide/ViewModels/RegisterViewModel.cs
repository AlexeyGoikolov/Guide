using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Guide.ViewModels
{
    public class RegisterViewModel
    {
       
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [EmailAddress(ErrorMessage = "Некорректный тип Email-a")]
        [Remote("CheckEmail", "Validation", ErrorMessage = "Этот Email занят")] 
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public int PositionsId { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}