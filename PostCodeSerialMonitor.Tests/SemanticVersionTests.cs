using System.Collections;
using PostCodeSerialMonitor.Models;

namespace PostCodeSerialMonitor.Tests;
public class SemanticVersionTestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {"0.2.0.0", "0.2.1.0", true},
        new object[] {"0.2.0.0", "0.2.10.0", true},
        new object[] {"0.2.0.0", "0.2.0.1", true},
        new object[] {"0.2.0.0", "0.3.0", true},
        new object[] {"0.2.0.0", "0.1.99", false},
        new object[] {"0.2.0.0", "0.1.33.0", false},
        new object[] {"0.3.0", "0.3.1", true},
        new object[] {"0.3.1.0", "0.3.11.0", true},
        new object[] {"0.3.2.0", "0.3.20.0", true},
        new object[] {"0.3.11.0", "0.3.1.0", false},

        new object[] {"1.0.0", "1.0.1", true},
        new object[] {"1.0.0", "1.1.0", true},
        new object[] {"1.0.0", "2.0.0", true},
        new object[] {"2.0.0", "1.9.9", false},
        new object[] {"1.2.3", "1.2.4", true},
        new object[] {"1.2.3", "1.3.0", true},
        new object[] {"1.2.3", "2.0.0", true},
        new object[] {"1.2.3", "1.2.3", false},
        new object[] {"10.0.0", "9.99.99", false},
        new object[] {"0.0.1", "0.0.2", true},
        new object[] {"0.0.1", "0.1.0", true},
        new object[] {"0.0.1", "1.0.0", true},

        new object[] {"v0.2.0.0", "v0.2.1.0", true},
        new object[] {"v0.2.0.0", "0.2.1.0", true},
        new object[] {"0.2.0.0", "v0.2.1.0", true},
        new object[] {"v1.0.0", "v1.0.1", true},
        new object[] {"v1.0.0", "1.0.1", true},
        new object[] {"1.0.0", "v1.0.1", true},
        new object[] {"v2.3.4", "v2.3.5", true},
        new object[] {"v2.3.4", "2.3.5", true},
        new object[] {"2.3.4", "v2.3.5", true},
        new object[] {"v10.20.30", "v10.20.31", true},
        new object[] {"v10.20.30", "10.20.31", true},
        new object[] {"10.20.30", "v10.20.31", true},

        new object[] {"v0.0.1", "v0.0.2", true},
        new object[] {"v0.0.1", "0.0.2", true},
        new object[] {"0.0.1", "v0.0.2", true},
        new object[] {"v1.2.3", "v1.2.3", false},
        new object[] {"v1.2.3", "1.2.3", false},
        new object[] {"1.2.3", "v1.2.3", false},

        new object[] {"0.1.0", "v0.2.0", true},
        new object[] {"v0.1.0", "0.2.0", true},
        new object[] {"0.1.0.0", "v0.2.0", true},
        new object[] {"v0.1.0.0", "0.2.0", true},
        new object[] {"1.0.0.0", "v1.1.0", true},
        new object[] {"v1.0.0.0", "1.1.0", true},

        new object[] {"0.9.9.9", "v1.0.0.0", true},
        new object[] {"v0.9.9.9", "1.0.0.0", true},
        new object[] {"1.99.99.99", "v2.0.0.0", true},
        new object[] {"v1.99.99.99", "2.0.0.0", true},
        new object[] {"0.0.0.1", "v0.0.0.2", true},
        new object[] {"v0.0.0.1", "0.0.0", false},
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class SemanticVersionTests
{
    [Fact]
    public void TestParsing()
    {
        Assert.Equal([0, 1, 2, 3], new SemanticVersion("v0.1.2.3").VersionParts);
        Assert.Equal([0, 1, 2, 3], new SemanticVersion("0.1.2.3").VersionParts);
        Assert.Equal([0, 1, 2, 0], new SemanticVersion("v0.1.2").VersionParts);
        Assert.Equal([0, 1, 2, 0], new SemanticVersion("0.1.2").VersionParts);
    }

    [Theory]
    [ClassData(typeof(SemanticVersionTestDataGenerator))]
    public void TestComparison(string localVersion, string remoteVersion, bool isNewer)
    {
        var local = new SemanticVersion(localVersion);
        var remote = new SemanticVersion(remoteVersion);

        Assert.Equal(isNewer, remote > local);
    }
}