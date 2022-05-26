using System.Text.Json.Serialization;

namespace MoneyPro2.Models;
public class User
{
    public User()
    {
        Roles = new List<Role>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Slug { get; set; }
    public DateTime ControlStart { get; set; }
    [JsonIgnore]
    public string PasswordHash { get; set; }
    public string Image { get; set; }
    public IList<Role> Roles { get; set; }
    [JsonIgnore]
    public IList<Login> Logins { get; set; }
}