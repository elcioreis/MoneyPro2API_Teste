using System.Text.Json.Serialization;

namespace MoneyPro2.Models;
public class User
{
    public User()
    {
        Name = String.Empty;
        Email = String.Empty;
        Slug = String.Empty;
        PasswordHash = String.Empty;
        Roles = new List<Role>();
        Logins = new List<Login>();
        InstitutionTypes = new List<InstitutionType>();
        Institutions = new List<Institution>();
        Entries = new List<Entry>();
        CategoryGroups = new List<CategoryGroup>();
        Categories = new List<Category>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Slug { get; set; }
    public DateTime ControlStart { get; set; }
    [JsonIgnore]
    public string PasswordHash { get; set; }
    public string? Image { get; set; }
    public IList<Role> Roles { get; set; }
    [JsonIgnore]
    public IList<Login> Logins { get; set; }
    [JsonIgnore]
    public IList<InstitutionType> InstitutionTypes { get; set; }
    [JsonIgnore]
    public IList<Institution> Institutions { get; set; }
    [JsonIgnore]
    public IList<Entry> Entries { get; set; }
    [JsonIgnore]
    public IList<CategoryGroup> CategoryGroups { get; set; }
    [JsonIgnore]
    public IList<Category> Categories { get; set; }
}