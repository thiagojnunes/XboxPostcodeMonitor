using System.Collections.Generic;
using Avalonia.X11.Interop;

namespace PostCodeSerialMonitor.Utils;

public class SemanticVersionUtils
{
  private string _version { get; set; } = string.Empty;

  public SemanticVersionUtils(string version)
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
  public static bool operator >(SemanticVersionUtils left, SemanticVersionUtils right)
  {
    return string.Compare(left._version, right._version) > 0;
  }

  /// Override less-than operator for SemanticVersionUtils
  public static bool operator <(SemanticVersionUtils left, SemanticVersionUtils right)
  {
    return string.Compare(left._version, right._version) < 0;
  }
}