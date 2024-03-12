﻿using Microsoft.EntityFrameworkCore;
using Serilog;
using WebBlog.Data;
using WebBlog.Entities;

namespace WebBlog.Repository;

public class IMemPostsRepository : IPostsRepository
{
    private readonly DataContext _data;
   
    public IMemPostsRepository(DataContext data)
    {
        _data = data;
    }

    public async Task<List<Post>> GetAll(int pageIndex)
    {
        int pageSize = 5;
        List<Post> posts = await _data.Posts.OrderByDescending(p => p.DatePosted)
            .Include(p => p.User)
            .AsNoTracking()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return posts;
    }

    public async Task<List<Post>> GetAllRoom(int roomId, int pageIndex)
    {
        try
        {
            int pageSize = 5;
            List<Post> posts = await _data.Posts.Include(o => o.Room)
                .Where(o => o.Room.Id == roomId)
                .OrderByDescending(p => p.DatePosted)
                .Include(p => p.User)
                .AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return posts;
        }
        catch (Exception ex)
        {
            Log.Error("Error fetching posts. {@error}", ex.Message);
            throw;
        }
    }

    public async Task<Post> GetOne(int id)
    {
        try
        {
            Post post = await _data.Posts.Include(p=>p.User).AsNoTracking().FirstOrDefaultAsync(p=>p.Id == id);
            return post;
        }
        catch (Exception ex)
        {
            Log.Error("Error fetching post. {@error}", ex.Message);
            throw;
        }
    }

    public async Task<Post> AddOne(CreatePostDto post)
    {
        try
        {
            Room room = await _data.Rooms.FindAsync(post.RoomId);
            User user = await _data.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == post.Author) ?? new User() { UserName = post.Author };
            Post newPost = new Post() { Title=post.Title, Body=post.Body, Image=post.Image, Room=room, RoomId=room.Id, User=user};
            await _data.Posts.AddAsync(newPost);
            await _data.SaveChangesAsync();
            return newPost;
        }
        catch (Exception ex)
        {
            Log.Error("Error creating post. {@error}", ex.Message);
            throw;
        }
    }

    public async Task UpdateOne(UpdatePostDto post)
    {
        try
        {
            Post dbPost = await _data.Posts.FindAsync(post.Id);
            dbPost.Title = post.Title;
            dbPost.Body = post.Body;
            await _data.SaveChangesAsync();
            return;
        }
        catch (Exception ex)
        {
            Log.Error("Error updating post. {@error}", ex.Message);
            throw;
        }
    }

    public async Task DeleteCommentAndReplies(Comment comment)
    {
        foreach(var reply in comment.Comments)
        {
            await DeleteCommentAndReplies(reply);
        }
        _data.Comments.Remove(comment);
    }

    public async Task DeleteOne(int id)
    {
        try
        {
            Post post = await _data.Posts.Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);


            foreach (var comment in post.Comments)
            {
                await DeleteCommentAndReplies(comment);
            }
            _data.Remove(post);
            await _data.SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Log.Error("Error deleting post. {@error}", ex.Message);
            throw;
        }
    }
}
