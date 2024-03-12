using Microsoft.EntityFrameworkCore;
using Serilog;
using WebBlog.Data;
using WebBlog.Entities;

namespace WebBlog.Repositories;

public class IMemRulesRepository : IRulesRepository
{
    private readonly DataContext _data;
    public IMemRulesRepository(DataContext data)
    {
        _data = data;
    }

    public async Task<List<Rule>> GetAll(int roomId)
    {
        try
        {
            List<Rule> rules = await _data.Rules.Include(o => o.Room)
                .Where(o => o.Room.Id == roomId)
                .AsNoTracking()
                .OrderBy(o => o.Order)
                .ToListAsync();
            return rules;
        }
        catch (Exception ex) {
            Log.Error("Error fetching rules. {@error}", ex.Message);
            throw;
        }
    }

    public async Task<Rule> GetOne(int id)
    {
        try
        {
            Rule rule = await _data.Rules.FindAsync(id);
            return rule;
        }
        catch (Exception ex) {
            Log.Error("Error fetching rule. {@error}", ex.Message);
            throw;
        }
    }

    public async Task<Rule> AddOne(CreateRuleDto rule)
    {
        try
        {
            Room room = await _data.Rooms.FindAsync(rule.RoomId);
            Rule newRule = new Rule() { RuleText=rule.RuleText, Room=room, RoomId=rule.RoomId};
            await _data.Rules.AddAsync(newRule);
            await _data.SaveChangesAsync();
            return newRule;
        }
        catch (Exception ex)
        {
            Log.Error("Error creating rule. {@error}", ex.Message);
            throw;
        }
    }

    public async Task UpdateOne(UpdateRuleDto rule)
    {
        try
        {
            Room room = await _data.Rooms.Include(r=>r.Rules).FirstOrDefaultAsync(r=>r.Id==rule.RoomId);

            var rulesToRemove = room.Rules.Where(r => !rule.RuleText.Contains(r.RuleText)).ToList();
            foreach (var ruleToRemove in rulesToRemove)
            {
                room.Rules.Remove(ruleToRemove);
                _data.Rules.Remove(ruleToRemove);
            }

            room.Rules.Clear();
            for (int i = 0; i < rule.RuleText.Count; i++)
            {
                var ruleText = rule.RuleText[i];
                Rule dbRule = await _data.Rules.FirstOrDefaultAsync((r) => r.RuleText == ruleText);
                if (dbRule == null)
                {
                    dbRule = new Rule() { RuleText = ruleText };
                }
                dbRule.Order = i;
                room.Rules.Add(dbRule);
            }
            await _data.SaveChangesAsync();
            return;
        }
        catch (Exception ex)
        {
            Log.Error("Error updating rule. {@error}", ex.Message);
            throw;
        }
    }
    public async Task DeleteOne(int id)
    {
        try
        {
            Rule rule = await _data.Rules.FindAsync(id);
            _data.Rules.Remove(rule);
            await _data.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error("Error deleting rule. {@error}", ex.Message);
            throw;
        }
    }
}