using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PostCodeSerialMonitor.Models;

public partial class SemanticVersion
{
  public const int MIN_PARTS = 3;
  public const int EXPECTED_PARTS = 4;
  public string Version { get; } = string.Empty;

  // Format: Major.Minor.Patch.<optional build>
  public List<int> VersionParts { get; } = [];

  [GeneratedRegex(@"^(?:v)?(\d+)\.(\d+)\.(\d+)\.?(\d+)?", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
  private static partial Regex VersionPartsRegex();

  public SemanticVersion(string version)
  {
    var match = VersionPartsRegex().Match(version);

    VersionParts = match.Groups.Values
      // Skip first entry (the full matched string!)
      .Skip(1)
      .Select(x => x.Success ? int.Parse(x.Value) : 0)
      .ToList();

    if (VersionParts.Count < MIN_PARTS)
    {
      throw new InvalidDataException("Expecting at least 3 numbers in SemanticVersion");
    }

    //Ignore the 'v' at the beginning of the version string
    if (version.StartsWith("v"))
    {
      Version = version.Substring(1);
    }
    else
    {
      Version = version;
    }
  }

  /// Override greater-than operator for SemanticVersion
  public static bool operator >(SemanticVersion left, SemanticVersion right)
  {
    for (int i = 0; i < EXPECTED_PARTS; i++)
    {
      if (left.VersionParts[i] > right.VersionParts[i]) return true;
      if (left.VersionParts[i] < right.VersionParts[i]) return false;
    }
    return false;
  }

  /// Override less-than operator for SemanticVersion
  public static bool operator <(SemanticVersion left, SemanticVersion right)
  {
    for (int i = 0; i < EXPECTED_PARTS; i++)
    {
      if (left.VersionParts[i] < right.VersionParts[i]) return true;
      if (left.VersionParts[i] > right.VersionParts[i]) return false;
    }
    return false;
  }
}