using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EF7API.Mdoels.Entities;

public class BlogDBContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public BlogDBContext()
    {
    }

    public BlogDBContext(DbContextOptions<BlogDBContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("BlogDB");
        }
    }
}
