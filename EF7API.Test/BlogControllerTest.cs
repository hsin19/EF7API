using EF7API.Controllers;
using EF7API.Mdoels.Entities;
using EF7API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Drawing.Printing;

namespace EF7API.Test;

public class BlogControllerTest
{
    private readonly DbContextOptions<BlogDBContext> _dbOptions;

    public BlogControllerTest()
    {
        _dbOptions = new DbContextOptionsBuilder<BlogDBContext>()
            .UseInMemoryDatabase("in-memory")
            .Options;
    }

    [Fact]
    public async Task Get_blog_items_success()
    {
        var controller = GetBlogController();
        var actionResult = await controller.GetBlogs();
        Assert.IsType<ActionResult<IEnumerable<Blog>>>(actionResult);
    }

    [Fact]
    public async Task Get_blog_item_success()
    {
        using var context = new BlogDBContext(_dbOptions);
        var blog = new Blog(Guid.NewGuid())
        {
            BlogContent = "ABCDEF",
            BlogTitle = "Title",
            UserId = "AAA"
        };
        context.Blogs.Add(blog);
        await context.SaveChangesAsync();

        var controller = GetBlogController();
        var actionResult = await controller.GetBlog(blog.BlogId);
        Assert.IsType<ActionResult<Blog>>(actionResult);
        var value = Assert.IsAssignableFrom<Blog>(actionResult.Value);
        Assert.Equal(blog.BlogContent, value.BlogContent);
        Assert.Equal(blog.BlogTitle, value.BlogTitle);
        Assert.Equal(blog.UserId, value.UserId);
    }

    private BlogController GetBlogController()
    {
        var context = new BlogDBContext(_dbOptions);

        var guidGeneratorMock = new Mock<GuidGenerator>();

        var blogController = new BlogController(context, guidGeneratorMock.Object);

        return blogController;
    }
}