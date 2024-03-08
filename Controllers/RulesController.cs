using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBlog.Entities;
using WebBlog.Repositories;

namespace WebBlog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RulesController : ControllerBase
{
    private readonly IRulesRepository _repository;
    public RulesController(IRulesRepository repo)
    {
        _repository = repo;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<List<Rule>>> GetAll(int id)
    {
        List<Rule> rules = await _repository.GetAll(id);
        return Ok(rules);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Rule>> AddOne(CreateRuleDto rule)
    {
        Rule dbRule = await _repository.AddOne(rule);
        return Ok(dbRule);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateOne(UpdateRuleDto rule)
    {
        await _repository.UpdateOne(rule);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOne(int id)
    {
        Rule rule = await _repository.GetOne(id);
        if(rule == null)
        {
            return NoContent();
        }
        await _repository.DeleteOne(id);
        return NoContent();
    }


}
