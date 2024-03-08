using System.ComponentModel.DataAnnotations;
using WebBlog.Entities;

namespace WebBlog;

public record RoomDto(
    int Id,
    string Name,
    string Description,
    User User,
    List<Post> Posts,
    List<Rule> Rules
);

public record CreateRoomDto(
    string Name,
    string Description,
    string UserName
);

public record UpdateRoomDto(
    int Id,
    string Description,
    string Name
);

public record PostDto(
    int Id,
    string Title,
    string Body,
    string Image,
    DateTime DatePosted,
    int? RoomId,
    Room Room,
    User User
);

public record CreatePostDto(
  string Title,
  string Body,
  string Author,
  string? Image,
  int RoomId
);

public record UpdatePostDto(
  string Title,
  string Body,
  string? Image,
  int Id
);

public record CommentDto(
    int Id,
    string Content,
    Post Post,
    int? ParentId,
    Comment ParentComment,
    List<Comment> Comments,
    User User
);

public record CreateCommentDto(
    string Content,
    string Author,
    int ParentId,
    int PostId
);

public record RuleDto(
   int Id,
   int? RoomId,
   Room Room,
   string RuleText
);

public record CreateRuleDto(
    int RoomId,
    string RuleText
);

public record UpdateRuleDto(
    int RoomId,
    List<string> RuleText
);

public record UserDto(
    int Id,
    string UserName,
    string ImageUrl,
    List<Post> Posts,
    List<Comment> Comments,
    List<Room> Rooms
);

public record UpdateUserDto(
    string UserName,
    string ImageUrl
);