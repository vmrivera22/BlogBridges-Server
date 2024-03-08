using Microsoft.EntityFrameworkCore;
using WebBlog.Data;
using WebBlog.Entities;
using WebBlog.Repository;

namespace WebBlog.Repositories;

public class IMemRoomsRepository : IRoomsRepository
{
    private readonly DataContext _data;
    private readonly IPostsRepository _posts;
    public IMemRoomsRepository(DataContext data, IPostsRepository repository) {
        _posts = repository;
        _data = data;
    }

    public async Task<List<Room>> GetAll()
    {
        List<Room> rooms = await _data.Rooms.Include(r=>r.User).ToListAsync();
        return rooms;
    }

    public async Task<Room> GetOne(int Id, int pageIndex)
    {
        int pageSize = 5;
        Room room = await _data.Rooms.Include(r => r.Posts.OrderByDescending(p => p.DatePosted).Skip((pageIndex - 1) * pageSize).Take(pageSize))
            .ThenInclude(p => p.User)
            .Include(r=>r.User)
            .FirstOrDefaultAsync(r => r.Id == Id);

        //Room room = await _data.Rooms.Include(r=>r.User).FirstOrDefaultAsync(r=>r.Id == Id);
        return room;
    }

    public async Task<Room> AddOne(CreateRoomDto room)
    {
        try
        {
            User user = await _data.Users.FirstOrDefaultAsync(u => u.UserName == room.UserName);
            if(user is null)
            {
                user = new User() { UserName = room.UserName};
            }
            Room newRoom = new Room() { Name=room.Name, Description=room.Description, User=user};
            _data.Rooms.Add(newRoom);
            await _data.SaveChangesAsync();
            return newRoom;
        }
        catch
        {
            throw;
        }
    }

    public async Task UpdateOne(UpdateRoomDto room)
    {
        try
        {
            Room dbRoom = await _data.Rooms.FindAsync(room.Id);
            dbRoom.Description = room.Description;
            dbRoom.Name = room.Name;
            await _data.SaveChangesAsync();
            return;
        }
        catch { throw; }
    }


    public async Task DeleteOne(int id)
    {
        try
        {
            Room room = await _data.Rooms.Include(p => p.Posts)
                .Include(p => p.Rules)
                .FirstOrDefaultAsync(p=>p.Id == id);

            for (int i = room.Posts.Count - 1; i >= 0; i--)
            {
                var post = room.Posts[i];
                await _posts.DeleteOne(post.Id);
            }
            for (int i = room.Rules.Count - 1; i>=0; i--)
            {
                _data.Remove(room.Rules[i]);
            }
            _data.Remove(room);
            await _data.SaveChangesAsync();
            return;
        }
        catch { throw; }
    }
}
