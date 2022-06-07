using System.ComponentModel.DataAnnotations;

namespace MoneyPro2.ViewModels.Entries;
public class EntryViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(40, ErrorMessage = "O nome não deve passar de 40 caracteres")]
    public string Name { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MaxLength(150, ErrorMessage = "A descrição não deve passar de 150 caracteres")]
    public string Description { get; set; }
}