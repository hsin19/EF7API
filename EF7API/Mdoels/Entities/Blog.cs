using System.Xml.Linq;

namespace EF7API.Mdoels.Entities;

public class Blog
{
    public Blog(Guid blogId)
    {
        BlogId = blogId;
    }

    public Guid BlogId { get; private set; }

    public string BlogTitle { get; set; } = default!;

    public string BlogContent { get; set; } = default!;

    public string UserId { get; set; } = default!;
}
