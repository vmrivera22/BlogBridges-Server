using System.ComponentModel.DataAnnotations;

namespace WebBlog.Entities;

public class Rule
{
    [Key]
    public int Id { get; set; }
    public int? RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public string RuleText {  get; set; }
    public int Order { get; set; }
}
