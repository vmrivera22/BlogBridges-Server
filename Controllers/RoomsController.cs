using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Entities;
using WebBlog.Repositories;

namespace WebBlog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomsRepository _repository;
    public RoomsController(IRoomsRepository repository)
    {
        _repository = repository;
    }
    [HttpGet]
    public async Task<ActionResult<List<Room>>> GetAll()
    {
        List<Room> rooms = await _repository.GetAll();
        return Ok(rooms);
    }
    [HttpGet("{id}/{page}")]
    public async Task<ActionResult<Room>> GetOne(int id, int page)
    {
        Room room = await _repository.GetOne(id, page);
        return Ok(room);
    }

    [HttpPost]
    public async Task<ActionResult<Room>> AddOne(CreateRoomDto room)
    {
        Room returnRoom = await _repository.AddOne(room);
        return Ok(returnRoom);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateOne(UpdateRoomDto room)
    {
        await _repository.UpdateOne(room);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOne(int id)
    {
        Room room = await _repository.GetOne(id, 1);
        if(room == null)
        {
            return NoContent();
        }
        await _repository.DeleteOne(id);
        return NoContent();
    }
}
