namespace PostCodeSerialMonitor.Models;

public class SemanticVersion
{
  private string _version { get; set; } = string.Empty;

  public SemanticVersion(string version)
  {
    //Ignore the 'v' at the beginning of the version string
    if (version.StartsWith("v"))
    {
      _version = version.Substring(1);
    }
    else
    {
      _version = version;
    }
  }

  /// Override greater-than operator for SemanticVersionUtils
  public static bool operator >(SemanticVersion left, SemanticVersion right)
  {
    return string.Compare(left._version, right._version) > 0;
  }

  /// Override less-than operator for SemanticVersionUtils
  public static bool operator <(SemanticVersion left, SemanticVersion right)
  {
    return string.Compare(left._version, right._version) < 0;
  }
}