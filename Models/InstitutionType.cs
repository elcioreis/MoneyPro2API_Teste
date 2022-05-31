using System.Text.Json.Serialization;

namespace MoneyPro2.Models;
public class InstitutionType
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public string Nickname { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
}
