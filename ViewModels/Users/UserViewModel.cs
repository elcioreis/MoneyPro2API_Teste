using System.ComponentModel.DataAnnotations;

namespace MoneyPro2.ViewModels.Users;
public class UserViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O e-mail é inválido")]
    public string Email { get; set; }
    public DateTime ControlStart { get; set; }
}

