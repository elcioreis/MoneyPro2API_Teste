using System.ComponentModel.DataAnnotations;

namespace MoneyPro2.ViewModels.Users;
public class RegisterViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O e-mail é inválido")]
    public string Email { get; set; }
    public DateTime ControlStart { get; set; }

    [RegularExpression(@"^^(?=(.*[a-z]){1,})(?=(.*[A-Z]){1,})(?=(.*[0-9]){1,})(?=(.*[!@#$%^&*()\-__+.]){1,}).{8,}$",
         ErrorMessage = "A senha deve conter números, maiúsculas, minúsculas, símbolos especiais e ao menos 8 caracteres.")]
    public string Password { get; set; }
}

