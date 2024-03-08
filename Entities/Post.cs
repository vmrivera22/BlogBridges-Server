using System.ComponentModel.DataAnnotations;

namespace WebBlog.Entities;

public class Post
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string? Image { get; set; }
    public DateTime DatePosted { get; set; } = DateTime.Now;

    public int? RoomId { get; set; }

    public Room Room { get; set; } = null!;
    public List<Comment>? Comments { get; set; } = new List<Comment>();

    public User User { get; set; } = null!;
}
