using System.ComponentModel.DataAnnotations;

namespace MoneyPro2.ViewModels.Coins;
public class CoinViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(25, ErrorMessage = "O nome não deve passar de 25 caracteres")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O símbolo é obrigatório")]
    [MaxLength(10, ErrorMessage = "O símbolo não deve passar de 10 caracteres")]
    public string Symbol { get; set; }
}
