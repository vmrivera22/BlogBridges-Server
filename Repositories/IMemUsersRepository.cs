using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Entities;

namespace WebBlog.Repositories;

public class IMemUsersRepository : IUsersRepository
{
    private readonly DataContext _data;
    public IMemUsersRepository(DataContext data)
    {
        _data = data;
    }

    public async Task<User> GetOne(string userName)
    {
        User user = _data.Users.FirstOrDefault(x => x.UserName == userName);
        return user;
    }

    public async Task<User> GetOnePosts(string userName, int pageIndex)
    {
        int pageSize = 5;
        User user = await _data.Users
            .Include(x => x.Posts.OrderByDescending(p=>p.DatePosted).Skip((pageIndex - 1) * pageSize)
            .Take(pageSize))
            .FirstOrDefaultAsync(u=>u.UserName == userName);
        return user;
    }

    public async Task UpdateOne(UpdateUserDto user)
    {
        User dbUser = _data.Users.FirstOrDefault(u=> u.UserName == user.UserName);
        if (dbUser == null)
        {
            dbUser = new User() { UserName=user.UserName, ImageUrl=user.ImageUrl};
            await _data.Users.AddAsync(dbUser);
        }
        else
        {
            dbUser.ImageUrl = user.ImageUrl;
        }
        await _data.SaveChangesAsync();
        return;
    }
}
