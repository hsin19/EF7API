using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace EF7API.Services;

public class GuidGenerator
{
    private readonly SequentialGuidValueGenerator _generator = new();

    public Guid NewGuild()
    {
        return _generator.Next(null!);
    }
}
