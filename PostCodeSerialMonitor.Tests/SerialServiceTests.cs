using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PostCodeSerialMonitor.Models;
using Xunit;

namespace PostCodeSerialMonitor.Tests;

/*
public class SerialServiceTests
{
    private readonly Mock<ISerialPort> _mockSerialPort;
    private readonly SerialService _serialService;

    public SerialServiceTests()
    {
        _mockSerialPort = new Mock<ISerialPort>();
        _serialService = new SerialService();
    }

    [Fact]
    public async Task Connect_ShouldSendResetSequence()
    {
        // Arrange
        var outputSequence = new[]
        {
            ">>version",
            "FW: v0.2.1 20240521",
            ">>config",
            "Notice: Showing config",
            "Display mirrored:       ON",
            "Disp rotation portrait: OFF",
            "Print timestamps:       ON",
            "Print colors:           ON",
            ">>"
        };

        var currentLine = 0;
        _mockSerialPort.Setup(x => x.ReadLine())
            .Returns(() => outputSequence[currentLine++]);

        // Act
        await _serialService.ConnectAsync("COM1");

        // Assert
        _mockSerialPort.Verify(x => x.Write("\x03"), Times.Once); // CTRL+C
        _mockSerialPort.Verify(x => x.Write("\r\n"), Times.Once); // ENTER
        _mockSerialPort.Verify(x => x.Write("version\r\n"), Times.Once);
        _mockSerialPort.Verify(x => x.Write("config\r\n"), Times.Once);
        _mockSerialPort.Verify(x => x.Write("post\r\n"), Times.Once);
    }

    [Fact]
    public async Task Connect_ShouldParseVersionInfo()
    {
        // Arrange
        var outputSequence = new[]
        {
            ">>",
            "FW: v0.2.1 20240521",
            ">>",
            "Notice: Showing config",
            "Display mirrored:       ON",
            "Disp rotation portrait: OFF",
            "Print timestamps:       ON",
            "Print colors:           ON",
            ">>"
        };

        var currentLine = 0;
        _mockSerialPort.Setup(x => x.ReadLine())
            .Returns(() => outputSequence[currentLine++]);

        // Act
        await _serialService.ConnectAsync("COM1");

        // Assert
        Assert.Equal("v0.2.1", _serialService.FirmwareVersion);
        Assert.Equal("20240521", _serialService.BuildDate);
    }

    [Fact]
    public async Task Connect_ShouldParseConfigState()
    {
        // Arrange
        var outputSequence = new[]
        {
            ">>",
            "FW: v0.2.1 20240521",
            "Notice: Showing config",
            "Display mirrored:       ON",
            "Disp rotation portrait: OFF",
            "Print timestamps:       ON",
            "Print colors:           ON",
            ">>"
        };

        var currentLine = 0;
        _mockSerialPort.Setup(x => x.ReadLine())
            .Returns(() => outputSequence[currentLine++]);

        // Act
        await _serialService.ConnectAsync("COM1");

        // Assert
        Assert.True(_serialService.MirrorDisplay);
        Assert.False(_serialService.PortraitMode);
        Assert.True(_serialService.PrintTimestamps);
    }

    [Fact]
    public async Task Connect_ShouldRetryOnFailedReset()
    {
        // Arrange
        var outputSequence = new[]
        {
            "some unexpected output",
            ">>",
            "FW: v0.2.1 20240521",
            "Notice: Showing config",
            "Display mirrored:       ON",
            "Disp rotation portrait: OFF",
            "Print timestamps:       ON",
            "Print colors:           ON",
            ">>"
        };

        var currentLine = 0;
        _mockSerialPort.Setup(x => x.ReadLine())
            .Returns(() => outputSequence[currentLine++]);

        // Act
        await _serialService.ConnectAsync("COM1");

        // Assert
        _mockSerialPort.Verify(x => x.Write("\x03"), Times.Exactly(2)); // CTRL+C twice
        _mockSerialPort.Verify(x => x.Write("\r\n"), Times.Exactly(2)); // ENTER twice
    }
}
*/
