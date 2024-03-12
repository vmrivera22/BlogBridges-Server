using Microsoft.EntityFrameworkCore;
using restaurant_server.Entities;
using WebBlog.Entities;

namespace WebBlog.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options): base(options) { }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Set up the room and post relationship.
        builder.Entity<Room>()
            .HasMany(e => e.Posts)
            .WithOne(e => e.Room)
            .HasForeignKey(e => e.RoomId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Set up the room and rules relationship.
        builder.Entity<Room>()
            .HasMany(e => e.Rules)
            .WithOne(e => e.Room)
            .HasForeignKey(e => e.RoomId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Set up the posts and comments relationship.
        builder.Entity<Post>()
            .HasMany(e=>e.Comments)
            .WithOne(e => e.Post)
            .HasForeignKey("PostId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Set up an index column for dateposted to make ordering by date quicker.
        builder.Entity<Post>()
            .HasIndex(e => e.DatePosted)
            .HasDatabaseName("IX_DatePosted");

        // Sut up the comments and comments (replies) relationship.
        builder.Entity<Comment>()
            .HasOne(e=>e.ParentComment)
            .WithMany(e=>e.Comments)
            .HasForeignKey(e=>e.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Set up the user and comments relationship.
        builder.Entity<User>()
            .HasMany(e => e.Comments)
            .WithOne(e => e.User)
            .HasForeignKey("UserId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Set up the user and posts relationship.
        builder.Entity<User>()
            .HasMany(e=>e.Posts)
            .WithOne(e=>e.User)
            .HasForeignKey("UserId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Set up the suer and rooms relationship.
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
