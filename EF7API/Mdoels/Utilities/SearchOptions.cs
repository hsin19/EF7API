using EF7API.Mdoels.Enums;
using System.Text.Json.Serialization;

namespace EF7API.Mdoels.Utilities;

public class SearchOptions
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Condition Condition { get; set; }

    public string? Value { get; set; }
}
