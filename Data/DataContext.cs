using Microsoft.EntityFrameworkCore;
using restaurant_server.Entities;
using WebBlog.Entities;

namespace WebBlog.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Room>()
            .HasMany(e => e.Posts)
            .WithOne(e => e.Room)
            .HasForeignKey(e => e.RoomId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Entity<Room>()
            .HasMany(e => e.Rules)
            .WithOne(e => e.Room)
            .HasForeignKey(e => e.RoomId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Entity<Post>()
            .HasMany(e=>e.Comments)
            .WithOne(e => e.Post)
            .HasForeignKey("PostId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Entity<Post>()
            .HasIndex(e => e.DatePosted)
            .HasName("IX_DatePosted");
        builder.Entity<Comment>()
            .HasOne(e=>e.ParentComment)
            .WithMany(e=>e.Comments)
            .HasForeignKey(e=>e.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Entity<User>()
            .HasMany(e => e.Comments)
            .WithOne(e => e.User)
            .HasForeignKey("UserId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Entity<User>()
            .HasMany(e=>e.Posts)
            .WithOne(e=>e.User)
            .HasForeignKey("UserId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.Entity<User>()
            .HasMany(e => e.Rooms)
            .WithOne(e => e.User)
            .HasForeignKey("UserId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }

}
