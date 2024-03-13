using Microsoft.EntityFrameworkCore;
using Serilog;
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

    // Get all rooms
    public async Task<List<Room>> GetAll()
    {
        try
        {
            List<Room> rooms = await _data.Rooms.AsNoTracking().ToListAsync();
            return rooms;
        }
        catch (Exception ex) {
            Log.Error("Error fetching rooms. {@error}", ex.Message);
            throw;
        }
    }

    // Get a single room and include a page of posts.
    public async Task<Room> GetOne(int Id, int pageIndex)
    {
        try
        {
            int pageSize = 5;
            var roomData = await _data.Rooms
                .Where(r => r.Id == Id)
                .Select(r => new
                {
                    Room = new
                    {
                        r.Id,
                        r.Name,
                        r.Description,
                        User = new
                        {
                            r.User.Id,
                            r.User.UserName,
                            r.User.ImageUrl
                        }
                    },
                    Posts = r.Posts
                    .OrderByDescending(p => p.DatePosted)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => new
                    {
                        p.Id,
                        p.Title,
                        p.Body,
                        p.Image,
                        p.DatePosted,
                        User = new
                        {
                            p.User.Id,
                            p.User.UserName,
                            p.User.ImageUrl
                        }
                    })
                    .ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
 
                // Map the anonymous type to the Room class
                var room = new Room
                {
                    Id = roomData.Room.Id,
                    Name = roomData.Room.Name,
                    Description = roomData.Room.Description,
                    User = new User 
                    { 
                        Id=roomData.Room.User.Id, 
                        ImageUrl=roomData.Room.User.ImageUrl, 
                        UserName=roomData.Room.User.UserName 
                    },
                    Posts = roomData.Posts.Select(post => new Post
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Body = post.Body,
                        Image = post.Image,
                        DatePosted = post.DatePosted,
                        User = new User
                        {
                            Id = post.User.Id,
                            UserName = post.User.UserName,
                            ImageUrl = post.User.ImageUrl
                        }
                    }).ToList()
                };
            return room;
        }
        catch (Exception ex)
        {
            Log.Error("Error fetching room. {@error}", ex.Message);
            throw;
        }
    }

    // Create a room.
    public async Task<Room> AddOne(CreateRoomDto room)
    {
        try
        {
            User user = await _data.Users.FirstOrDefaultAsync(u => u.UserName == room.UserName);
            if (user is null)
            {
                user = new User() { UserName = room.UserName };
            }
            Room newRoom = new Room() { Name=room.Name, Description=room.Description, User=user};
            _data.Rooms.Add(newRoom);
            await _data.SaveChangesAsync();
            return newRoom;
        }
        catch(Exception ex)
        {
            Log.Error("Error creating room. {@error}", ex.Message);
            throw;
        }
    }

    // Update a room.
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
        catch (Exception ex) 
        {
            Log.Error("Error updating room. {@error}", ex.Message);
            throw; 
        }
    }

    
    // Delete a room.
    public async Task DeleteOne(int id)
    {
        try
        {
            var room = await _data.Rooms.Include(p => p.Posts)
                .Include(p => p.Rules)
                .FirstOrDefaultAsync(p=>p.Id == id);
            if(room == null)
            {
                return;
            }

            // Delete a rooms posts.
            for (int i = room.Posts.Count - 1; i >= 0; i--)
            {
                var post = room.Posts[i];
                await _posts.DeleteOne(post.Id);
            }

            // Delete a rooms rules.
            for (int i = room.Rules.Count - 1; i>=0; i--)
            {
                _data.Remove(room.Rules[i]);
            }
            _data.Remove(room);
            await _data.SaveChangesAsync();
            return;
        }
        catch (Exception ex)
        {
            Log.Error("Error deleting room. {@error}", ex.Message);
            throw; 
        }
    }
}
