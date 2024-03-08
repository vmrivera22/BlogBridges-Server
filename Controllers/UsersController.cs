using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Entities;
using WebBlog.Repositories;

namespace WebBlog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersRepository _repository;
    public UsersController(IUsersRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{userName}/{page}")]
    public async Task<ActionResult<User>> GetOne(string userName, int page)
    {
        User user = null;
        if (page == -1) {
           user = await _repository.GetOne(userName);
        }
        else
        {
            user = await _repository.GetOnePosts(userName, page);
        }
        return Ok(user);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateOne(UpdateUserDto user)
    {
        await _repository.UpdateOne(user);
        return NoContent();
    }

}
