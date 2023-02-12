namespace EF7API.Mdoels.Entities;

public class Comment
{
    public Guid CommentId { get; set; }

    public string CommentText { get; set; } = default!;

    public string UserId { get; set; } = default!;
}
