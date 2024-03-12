using Microsoft.EntityFrameworkCore;
using Serilog;
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
        try
        {
            return await GetAllCommentsAndRepliesRecursive(null, PostId);
        }
        catch (Exception ex)
        {
            Log.Error("Error fetching comments. {@error}", ex.Message);
            throw;
        }
    }

    public async Task<List<Comment>> GetAllCommentsAndRepliesRecursive(int? ParentId, int PostId)
    {
        try
        {
            var comments = await _data.Comments
            .Where(c => c.Post.Id == PostId && c.ParentId == ParentId)
            .Include(c => c.User)
            .AsSplitQuery()
            .AsNoTracking()
            .OrderByDescending(c => c.DatePosted)
            .Take(5)
            .ToListAsync();

            foreach (var comment in comments)
            {
                comment.Comments = await GetAllCommentsAndRepliesRecursive(comment.Id, PostId);
            }

            return comments;
        }
        catch (Exception ex){
            Log.Error("Error fetching comments. {@error}", ex.Message);
            throw;
        }   
    }

    public async Task<Comment> AddOne(CreateCommentDto comment)
    {
        try
        {
            Comment parent = await _data.Comments.FindAsync(comment.ParentId);
            Post post = await _data.Posts.FindAsync(comment.PostId);
            User user = await _data.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == comment.Author) ?? new User() { UserName = comment.Author };
            Comment newComment;
            if (parent != null)
            {
                newComment = new Comment() { Content = comment.Content, Post = post, ParentComment = parent, User = user };
            }
            else
            {
                newComment = new Comment() { Content = comment.Content, Post = post, User = user };
                newComment = new Comment() { Content = comment.Content, Post = post, User = user };
            }
            await _data.Comments.AddAsync(newComment);
            await _data.SaveChangesAsync();
            return newComment;
        }
        catch(Exception ex)
        {
            Log.Error("Error creating comment. {@error}", ex.Message);
            throw;
        }
    }

    public async Task DeleteOne(int id)
    {
        try
        {
            Comment comment = await _data.Comments.FindAsync(id);
            _data.Comments.Remove(comment);
            await _data.SaveChangesAsync();
        }
        catch(Exception ex) {
            Log.Error("Error deleting comments. {@error}", ex.Message);
            throw;
        }
    }
}
