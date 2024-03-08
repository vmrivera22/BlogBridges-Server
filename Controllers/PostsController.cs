using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Entities;
using WebBlog.Repository;

namespace WebBlog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IPostsRepository _repository;
    public PostsController(IPostsRepository repository)
    {
        _repository = repository;
    }
    [HttpGet("{room}/{id}/{page}")]
    public async Task<ActionResult<List<Post>>> GetAllRoom(int room, int id, int page)
    {
        List<Post> posts = [];
        if (room == -1 && id == -1)
        {
            posts = await _repository.GetAll(page);
        }
        else if (room != -1 && id == -1)
        {
            posts = await _repository.GetAllRoom(room, page);
        }
        else if (id != -1)
        {
            Post post = await _repository.GetOne(id);
            posts.Add(post);
        }
        return Ok(posts);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> AddOne(CreatePostDto post)
    {
        Post newPost = await _repository.AddOne(post);
        return Ok(newPost);
    }


    [HttpPut]
    public async Task<ActionResult> UpdateOne(UpdatePostDto post)
    {
        await _repository.UpdateOne(post);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOne(int id)
    {
        Post post = await _repository.GetOne(id);
        if (post == null)
        {
            return NoContent();
        }
        await _repository.DeleteOne(id);
        return NoContent();
    }

}
