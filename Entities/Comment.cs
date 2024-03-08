namespace WebBlog.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime DatePosted { get; set; } = DateTime.Now;
    public User User { get; set; }
    public Post? Post { get; set; } = null!;
    public int? ParentId { get; set; }
    public Comment? ParentComment { get; set; }
    public List<Comment> Comments { get; set; } = new List<Comment>();
}
