using System.ComponentModel.DataAnnotations;

namespace MoneyPro2.ViewModels.Institution;
public class InstitutionTypeViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(25, ErrorMessage = "O nome não deve passar de 25 caracteres")]
    public string Name { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MaxLength(100, ErrorMessage = "A descrição não deve passar de 100 caracteres")]
    public string Description { get; set; }
}

