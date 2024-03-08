namespace WebBlog.Entities;

public static class EntityExtension
{
    public static RoomDto AsDto(this Room room)
    {
        return new RoomDto(
            room.Id,
            room.Name,
            room.Description,
            room.User,
            room.Posts,
            room.Rules
        );
    }
    public static PostDto AsDto(this Post post)
    {
        return new PostDto(
            post.Id,
            post.Title,
            post.Body,
            post.Image,
            post.DatePosted,
            post.RoomId,
            post.Room,
            post.User
        );
    }
    public static CommentDto AsDto(this Comment comment)
    {
        return new CommentDto(
            comment.Id,
            comment.Content,
            comment.Post,
            comment.ParentId,
            comment.ParentComment,
            comment.Comments,
            comment.User
        );
    }
    public static RuleDto AsDto(this Rule rule)
    {
        return new RuleDto(
            rule.Id,
            rule.RoomId,
            rule.Room,
            rule.RuleText
        );
    }
    public static UserDto AsDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.UserName,
            user.ImageUrl,
            user.Posts,
            user.Comments,
            user.Rooms
        );
    }
}
