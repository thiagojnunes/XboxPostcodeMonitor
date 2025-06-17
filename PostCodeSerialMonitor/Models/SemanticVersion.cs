namespace PostCodeSerialMonitor.Models;

public class SemanticVersion
{
  public string Version { get; } = string.Empty;

  public SemanticVersion(string version)
  {
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

  /// Override greater-than operator for SemanticVersionUtils
  public static bool operator >(SemanticVersion left, SemanticVersion right)
  {
    return string.Compare(left.Version, right.Version) > 0;
  }

  /// Override less-than operator for SemanticVersionUtils
  public static bool operator <(SemanticVersion left, SemanticVersion right)
  {
    return string.Compare(left.Version, right.Version) < 0;
  }
}