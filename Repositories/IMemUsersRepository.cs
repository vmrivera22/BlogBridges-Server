using Microsoft.EntityFrameworkCore;
using Serilog;
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
        try
        {
            User user = _data.Users.AsNoTracking().FirstOrDefault(x => x.UserName == userName) ?? new User() { UserName = "Error getting user" };
            return user;
        }
        catch (Exception ex)
        {
            Log.Error("Error fetching user. {@error}", ex.Message);
            throw;
        }
    }

    public async Task<User> GetOnePosts(string userName, int pageIndex)
    {
        try
        {
            int pageSize = 5;
            User user = await _data.Users
                .Include(x => x.Posts.OrderByDescending(p => p.DatePosted).Skip((pageIndex - 1) * pageSize)
                .Take(pageSize))
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName);
            return user;
        }
        catch(Exception ex)
        {
            Log.Error("Error fetching user. {@error}", ex.Message);
            throw;
        }
    }

    public async Task UpdateOne(UpdateUserDto user)
    {
        try
        {
            User dbUser = _data.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (dbUser == null)
            {
                dbUser = new User() { UserName = user.UserName, ImageUrl = user.ImageUrl };
                await _data.Users.AddAsync(dbUser);
            }
            else
            {
                dbUser.ImageUrl = user.ImageUrl;
            }
            await _data.SaveChangesAsync();
            return;
        }
        catch (Exception ex)
        {
            Log.Error("Error updating user. {@error}", ex.Message);
            throw;
        }
    }
}
