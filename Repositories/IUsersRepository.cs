using WebBlog.Entities;

namespace WebBlog.Repositories;

public interface IUsersRepository
{
    Task<User> GetOne(string userName);
    Task<User> GetOnePosts(string userName, int pageIndex);
    Task UpdateOne(UpdateUserDto user);
}
