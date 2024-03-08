using System.ComponentModel.DataAnnotations;

namespace WebBlog.Entities;

public class Room
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public User User { get; set; }

    public List<Post> Posts { get; } = [];

    public List<Rule> Rules { get; } = [];
}
