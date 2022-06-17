using System.Text.Json.Serialization;

namespace MoneyPro2.Models;
public class Category
{
    public Category()
    {
        Children = new List<Category>();
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? CategoryParentId { get; set; }
    [JsonIgnore]
    public Category CategoryParent { get; set; }
    public int? CategoryGroupId { get; set; }
    [JsonIgnore]
    public CategoryGroup CategoryGroup { get; set; }
    public string CrdDeb { get; set; }
    public int? VisualOrder { get; set; }
    public bool Fixed { get; set; }
    public bool System { get; set; }
    public bool Active { get; set; }
    public IList<Category> Children { get; set; }
}