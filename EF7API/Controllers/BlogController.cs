using EF7API.Mdoels.Dtos;
using EF7API.Mdoels.Entities;
using EF7API.Mdoels.Enums;
using EF7API.Mdoels.Utilities;
using EF7API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EF7API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogController : ControllerBase
    {
        private readonly BlogDBContext _context;
        private readonly GuidGenerator _guidGenerator;

        public BlogController(BlogDBContext context, GuidGenerator guidGenerator)
        {
            _context = context;
            _guidGenerator = guidGenerator;
        }

        // GET: api/Blog
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            return await _context.Blogs.ToListAsync();
        }

        // GET: api/Blog/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Blog>> GetBlog(Guid id)
        {
            Blog? blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }

        [HttpPost("Search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Blog>>> SearchBlogs(SearchOptions searchDto)
        {
            switch (searchDto.Condition)
            {
                case Condition.Contains when searchDto.Value != null:
                    return await _context.Blogs
                        .Where(b => b.BlogTitle.Contains(searchDto.Value))
                        .ToListAsync();
                case Condition.Equal when searchDto.Value != null:
                    return await _context.Blogs
                        .Where(b => b.BlogTitle == searchDto.Value)
                        .ToListAsync();
                default:
                    return BadRequest();
            }
        }

        // PUT: api/Blog/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog(Guid id, BlogDto blogDto)
        {
            Blog blog = ConvertToBlog(blogDto, id);

            // TODO Check user
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userId?.Value != blog.UserId)
            {
                return Unauthorized();
            }

            _context.Entry(blog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Blog
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog(BlogDto blogDto)
        {
            Blog blog = ConvertToBlog(blogDto);
            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlog), new { id = blog.BlogId }, blog);
        }

        // DELETE: api/Blog/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            Blog? blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            // not support InMemory
            //await _context.Comments.Where(c => c.BlogId == blog.BlogId).ExecuteDeleteAsync();
            List<Comment> comments = await _context.Comments.Where(c => c.BlogId == blog.BlogId).ToListAsync();

            _context.Blogs.Remove(blog);
            _context.Comments.RemoveRange(comments);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(Guid id)
        {
            return _context.Blogs.Any(e => e.BlogId == id);
        }

        private Blog ConvertToBlog(BlogDto blogDto, Guid? id = null)
        {
            id ??= _guidGenerator.NewGuild();
            string? name = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (name == null)
            {
                throw new Exception();
            }
            return new Blog(id.Value)
            {
                BlogContent = blogDto.BlogContent,
                BlogTitle = blogDto.BlogTitle,
                UserId = name
            };
        }
    }
}
