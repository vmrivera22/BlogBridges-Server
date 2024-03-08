namespace WebBlog.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string ImageUrl { get; set; } = "";

    public List<Post> Posts { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];

    public List<Room> Rooms { get; set; } = [];
}
