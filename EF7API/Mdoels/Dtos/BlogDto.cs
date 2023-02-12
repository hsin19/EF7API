using EF7API.Mdoels.Entities;
using Microsoft.Build.Framework;

namespace EF7API.Mdoels.Dtos;

public class BlogDto
{
    [Required]
    public string BlogTitle { get; set; } = default!;

    [Required]
    public string BlogContent { get; set; } = default!;
}
