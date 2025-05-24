using System.Collections;
using PostCodeSerialMonitor.Models;
using PostCodeSerialMonitor.Services;
using System.IO;
using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace PostCodeSerialMonitor.Tests;
public class TestDataGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> _data = new List<object[]>
    {
        new object[] {"CPU (1): 0x14ff [2BL_FINAL_SUCCESS] (6683 ms)", new DecodedCode(){
            Flavor = CodeFlavor.CPU,
            Index = 1,
            Code = 0x14ff
        }},
        new object[] {"SP  (1): 0x0075 [BOOT_SUCCESS] (2423 ms)", new DecodedCode(){
            Flavor = CodeFlavor.SP,
            Index = 1,
            Code = 0x0075
        }}
    };

    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class SerialDecoderTests
{
    private readonly SerialLineDecoder _decoder;
    private readonly string _testDataPath;

    public SerialDecoderTests()
    {
        _testDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "postcodes.csv");
        var csvContent = File.ReadAllText(_testDataPath);
        
        // Create configuration
        var config = new AppConfiguration();
        var options = Options.Create(config);

        // Create logger factory
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        
        // Create configuration service
        var configService = new ConfigurationService(
            options,
            loggerFactory.CreateLogger<ConfigurationService>(),
            Path.GetDirectoryName(_testDataPath) ?? "."
        );

        // Create JSON options
        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            AllowTrailingCommas = true,
            WriteIndented = true
        };

        // Create MetaUpdateService
        var metaUpdateService = new MetaUpdateService(
            configService,
            jsonOptions,
            loggerFactory.CreateLogger<MetaUpdateService>()
        );

        // Create MetaDefinitionService
        var metaDefinitionService = new MetaDefinitionService(configService, metaUpdateService);
        metaDefinitionService.RefreshMetaDefinitionsAsync().GetAwaiter().GetResult();

        _decoder = new SerialLineDecoder(metaDefinitionService);
    }

    [Theory]
    [ClassData(typeof(TestDataGenerator))]
    public void TestDecoding(string input, DecodedCode expected)
    {
        var result = _decoder.DecodeLine(input, ConsoleType.XboxOne);
        Assert.NotNull(result);
        Assert.Equal(expected.Flavor, result.Flavor);
        Assert.Equal(expected.Index, result.Index);
        Assert.Equal(expected.Code, result.Code);
    }
}