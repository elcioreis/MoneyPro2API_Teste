using System.Text.Json.Serialization;

namespace MoneyPro2.Models;

public class Institution
{
    public Institution()
    {
        Nickname = String.Empty;
        Description = String.Empty;
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
    public int InstitutionTypeId { get; set; }
    [JsonIgnore]
    public InstitutionType? InstitutionType { get; set; }
    public string Nickname { get; set; }
    public string Description { get; set; }
    public int? BankNumber { get; set; }
    public bool Active { get; set; }
}
