using System.ComponentModel.DataAnnotations;


namespace Guide.ViewModels
{
    public class EditUserViewModel
    {
            public string Id { get; set; }
            [Required(ErrorMessage = "Это поле обязательно для заполнения")]
            [EmailAddress(ErrorMessage = "Некорректный тип Email-a")]
            public string Email { get; set; }
            [Required(ErrorMessage = "Это поле обязательно для заполнения")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Это поле обязательно для заполнения")]
            public string Surname { get; set; }
            [Required(ErrorMessage = "Это поле обязательно для заполнения")]
            public int PositionsId { get; set; }
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            [DataType(DataType.Password)]
            public string PasswordConfirm { get; set; }
            public Roles Role { get; set; }
 
    }
}