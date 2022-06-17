using System.Text.Json.Serialization;

namespace MoneyPro2.Models;
public class CategoryGroup
{
    public CategoryGroup()
    {
        Categories = new List<Category>();
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Active { get; set; }
    [JsonIgnore]
    public IList<Category> Categories { get; set; }
}