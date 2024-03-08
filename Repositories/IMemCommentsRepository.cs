using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Entities;

namespace WebBlog.Repositories;

public class IMemCommentsRepository : ICommentsRepository
{
    private readonly DataContext _data;

    public IMemCommentsRepository(DataContext data)
    {
        _data = data;
    }

    public async Task<List<Comment>> GetAll(int PostId)
    {
        return await GetAllCommentsAndRepliesRecursive(null, PostId);
    }

    public async Task<List<Comment>> GetAllCommentsAndRepliesRecursive(int? ParentId, int PostId)
    {
        var comments = await _data.Comments
        .Where(c => c.Post.Id == PostId && c.ParentId == ParentId)
        .Include(c => c.User)
        .OrderByDescending(c=>c.DatePosted)
        .Take(5)
        .ToListAsync();

        foreach (var comment in comments)
        {
            comment.Comments = await GetAllCommentsAndRepliesRecursive(comment.Id, PostId);
        }

        return comments;
    }

    public async Task<Comment> AddOne(CreateCommentDto comment)
    {
        Comment Parent = await _data.Comments.FindAsync(comment.ParentId);
        Post Post = await _data.Posts.FindAsync(comment.PostId);
        User User = await _data.Users.FirstOrDefaultAsync(u=>u.UserName == comment.Author);

        if (User == null)
        {
            User = new User() { UserName=comment.Author };
        }
        Comment newComment;
        if (Parent != null)
        {
            newComment = new Comment() { Content = comment.Content, Post=Post, ParentComment=Parent, User=User};
        }
        else
        {
            newComment = new Comment() { Content = comment.Content, Post = Post, User=User };
        }
        await _data.Comments.AddAsync(newComment);
        await _data.SaveChangesAsync();
        return newComment;
    }

    public async Task DeleteOne(int id)
    {
        Comment comment = await _data.Comments.FindAsync(id);
        _data.Comments.Remove(comment);
        await _data.SaveChangesAsync();
    }
}
