using WebBlog.Entities;

namespace WebBlog.Repositories;

public interface IRoomsRepository
{
    Task<List<Room>> GetAll();
    Task<Room> GetOne(int id, int pageIndex);
    Task<Room> AddOne(CreateRoomDto room);
    Task UpdateOne(UpdateRoomDto room); 
    Task DeleteOne(int id);
}
