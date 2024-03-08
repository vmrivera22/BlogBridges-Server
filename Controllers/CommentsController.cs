using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Entities;
using WebBlog.Repositories;

namespace WebBlog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentsRepository _repository;
    public CommentsController(ICommentsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{PostId}")]
    public async Task<ActionResult<List<Comment>>> GetAllComment(int PostId)
    {
        List<Comment> comments = await _repository.GetAll(PostId);
        return Ok(comments);
    }

   
    [HttpPost]
    public async Task<ActionResult<Comment>> AddOneComment(CreateCommentDto comment)
    {
        Comment retunComment = await _repository.AddOne(comment);
        return Ok(retunComment);

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOneComment(int id)
    {
        await _repository.DeleteOne(id);
        return NoContent();
    }
}
