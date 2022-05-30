using System.ComponentModel.DataAnnotations;

namespace MoneyPro2.ViewModels.InstitutionType;
public class InstitutionTypeViewModel
{
    [Required(ErrorMessage = "O apelido é obrigatório")]
    [MaxLength(25, ErrorMessage = "O apelido não deve passar de 25 caracteres")]
    public string Nickname { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MaxLength(100, ErrorMessage = "A descrição não deve passar de 100 caracteres")]
    public string Description { get; set; }
}

