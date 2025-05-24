using System;
using System.Text.Json.Serialization;

namespace PostCodeSerialMonitor.Models;

public class MetaDefinition
{
    public int FormatVersion { get; set; }
    public DateTime Updated { get; set; }
    public MetaEntry[] Items { get; set; } = [];

    /// Override greater-than operator for MetaDefinition
    public static bool operator >(MetaDefinition left, MetaDefinition right)
    {
        return left.Updated > right.Updated;
    }

    /// Override greater-than operator for MetaDefinition
    public static bool operator <(MetaDefinition left, MetaDefinition right)
    {
        return left.Updated < right.Updated;
    }
}

public class MetaEntry
{
    public MetaType MetaType { get; set; }
    public string Path { get; set; } = string.Empty;
    [JsonConverter(typeof(Base64UrlByteArrayConverter))]
    public byte[] Hash { get; set; } = [];
}

public enum MetaType {
    PostCodes,
    OSErrors,
    ErrorMasks
}