using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Guide.Models;
using Microsoft.AspNetCore.Mvc;

namespace Guide.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Некорректный тип Email-a")]
        [Remote("CheckEmail", "Validation", ErrorMessage = "Этот Email уже используется")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        public string Surname { get; set; }

        public int? PositionsId { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Минимальная длина пароля 6 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public Roles Role { get; set; }
        public List<Position> Positions { get; set; }
        public EditUserViewModel UserEdit { get; set; }
    }
}