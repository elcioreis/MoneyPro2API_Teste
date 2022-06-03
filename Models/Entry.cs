using System.Text.Json.Serialization;

namespace MoneyPro2.Models;

public class Entry
{
    public int Id { get; set; }
    public int UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool Active { get; set; }
    public bool System { get; set; }
}