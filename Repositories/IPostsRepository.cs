using WebBlog.Entities;

namespace WebBlog.Repository;

public interface IPostsRepository
{
    Task<List<Post>> GetAll(int pageIndex);
    Task<List<Post>> GetAllRoom(int roomId, int pageIndex);
    Task<Post> GetOne(int id);
    Task<Post> AddOne(CreatePostDto post);
    Task UpdateOne(UpdatePostDto post);
    Task DeleteOne(int id);
}
