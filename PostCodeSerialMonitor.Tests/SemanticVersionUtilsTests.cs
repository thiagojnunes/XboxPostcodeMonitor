using System.Collections;
using PostCodeSerialMonitor.Utils;

namespace PostCodeSerialMonitor.Tests;
public class SemanticVersionTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {"0.2.0.0", "0.2.1.0", true},
        new object[] {"0.2.0.0", "0.2.10.0", true},
        new object[] {"0.2.0.0", "0.2.0.1", true},
        new object[] {"0.2.0.0", "0.3.0", true},
        new object[] {"0.2.0.0", "0.1.33.0", false},
        new object[] {"0.3.0", "0.3.1", true},
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class SemanticVersionUtilsTests
{
    [Theory]
    [ClassData(typeof(SemanticVersionTestDataGenerator))]
    public void TestComparison(string localVersion, string remoteVersion, bool isNewer)
    {
        var local = new SemanticVersionUtils(localVersion);
        var remote = new SemanticVersionUtils(remoteVersion);

        Assert.Equal(isNewer, remote > local);
    }
}