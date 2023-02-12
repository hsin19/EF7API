using Microsoft.EntityFrameworkCore;

namespace EF7API.Mdoels.Entities;

public class BlogDBContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
}
