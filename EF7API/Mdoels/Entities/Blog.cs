namespace EF7API.Mdoels.Entities;

public class Blog
{
    public Guid BlogId { get; set; }

    public string BlogTitle { get; set; } = default!;

    public string BlogContent { get; set; } = default!;

    public string UserId { get; set; } = default!;
}
