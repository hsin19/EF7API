using EF7API.Services;
using EF7API.Services.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EF7API.Extensions;

public static class JwtServiceExtension
{
    public static AuthenticationBuilder AddConfigJwt(this AuthenticationBuilder builder, IConfiguration? configuration = null, string? authenticationScheme = null)
    {
        authenticationScheme ??= JwtBearerDefaults.AuthenticationScheme;
        if (configuration != null)
        {
            builder.Services.Configure<MyJwtOptions>(configuration);
        }
        builder.Services.TryAddSingleton<JwtService>();
        builder.Services.AddOptions<JwtBearerOptions>(authenticationScheme)
            .Configure<JwtService>((options, service) =>
            {
                service.Configure(options);
            });
        builder.AddJwtBearer(authenticationScheme, _ => { });
        return builder;
    }
}
