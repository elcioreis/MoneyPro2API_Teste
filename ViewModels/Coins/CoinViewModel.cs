using System.ComponentModel.DataAnnotations;

namespace MoneyPro2.ViewModels.Coins;
public class CoinViewModel
{
    [Required(ErrorMessage = "O apelido é obrigatório")]
    [MaxLength(25, ErrorMessage = "O apelido não deve passar de 25 caracteres")]
    public string Nickname { get; set; }

    [Required(ErrorMessage = "O símbolo é obrigatório")]
    [MaxLength(25, ErrorMessage = "O símbolo não deve passar de 10 caracteres")]
    public string Symbol { get; set; }
}
