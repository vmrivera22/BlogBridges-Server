using WebBlog.Entities;

namespace WebBlog.Repositories;

public interface ICommentsRepository
{
    Task<List<Comment>> GetAll(int PostId);
    Task<Comment> AddOne(CreateCommentDto comment);

    Task DeleteOne(int id);
}
