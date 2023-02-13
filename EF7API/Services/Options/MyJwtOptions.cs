namespace EF7API.Services.Options;

public class MyJwtOptions
{
    public string? Issuer { set; get; } = nameof(EF7API);

    public IEnumerable<string>? ValidIssuers { set; get; }

    private static string? s_randomSignKey;

    public string SignKey { set; get; }

    public int? ExpireTime { set; get; }

    public string? Audience { set; get; }

    public IEnumerable<string>? ValidAudiences { set; get; }

    public MyJwtOptions()
    {
        s_randomSignKey ??= Convert.ToBase64String(Guid.NewGuid().ToByteArray()).TrimEnd('=');
        SignKey ??= s_randomSignKey;
    }
}
