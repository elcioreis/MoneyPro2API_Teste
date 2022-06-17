using System.ComponentModel.DataAnnotations;

namespace MoneyPro2.ViewModels.Category;
public class CategoryViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [MaxLength(40, ErrorMessage = "O nome não deve passar de 40 caracteres")]
    public string Name { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [MaxLength(150, ErrorMessage = "A descrição não deve passar de 150 caracteres")]
    public string Description { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "A identificação da categoria pai deve ser maior ou igual a zero")]
    public int CategoryParentId { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O grupo de categorias deve ser maior maior ou igual a zero")]
    public int CategoryGroupId { get; set; }

    [Required(ErrorMessage = "O campo CrdDeb é obrigatório")]
    [RegularExpression(@"^[CDITM]{1}$",
        ErrorMessage = "O campo CrdDev deve ser (C)rédito, (D)ébito, (I)nvestimento, (T)ransferência ou (M)anter igual ao pai.")]
    public string CrdDeb { get; set; }

    [Required(ErrorMessage = "O campo Fixed é obrigatório")]
    [RegularExpression(@"^(true|false)$", ErrorMessage = "O campo Fixed deve ser 'true' ou 'false'")]
    public string Fixed { get; set; }

    [Required(ErrorMessage = "O campo System é obrigatório")]
    [RegularExpression(@"^(true|false)$", ErrorMessage = "O campo System deve ser 'true' ou 'false'")]
    public string System { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "A ordem visual deve ser maior ou igual a zero")]
    public int VisualOrder { get; set; }
}