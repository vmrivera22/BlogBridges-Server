using WebBlog.Entities;

namespace WebBlog.Repositories;

public interface IRulesRepository
{
    Task<List<Rule>> GetAll(int roomId);
    Task<Rule> GetOne(int id);
    Task<Rule> AddOne(CreateRuleDto rule);
    Task UpdateOne(UpdateRuleDto rule);
    Task DeleteOne(int id);
}
