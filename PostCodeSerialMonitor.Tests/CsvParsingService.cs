using System;
using System.IO;
using System.Linq;
using Xunit;
using PostCodeSerialMonitor.Models;
using PostCodeSerialMonitor.Services;

namespace PostCodeSerialMonitor.Tests;
public class CsvParsingServiceTests
{
    private readonly string _testDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "postcodes.csv");

    [Fact]
    public void ParsePostCodes_ShouldParseAllEntries()
    {
        // Arrange
        var csvContent = File.ReadAllText(_testDataPath);

        // Act
        var result = CsvParsingService.ParsePostCodes(csvContent);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(56, result.Count()); // Total number of entries in the CSV file
    }

    [Fact]
    public void ParsePostCodes_ShouldParseSMCErrorCode()
    {
        // Arrange
        var csvContent = File.ReadAllText(_testDataPath);

        // Act
        var result = CsvParsingService.ParsePostCodes(csvContent);
        var smcError = result.FirstOrDefault(x => x.CodeFlavor == CodeFlavor.SMC && x.Name == "FATAL_V12");

        // Assert
        Assert.NotNull(smcError);
        Assert.Equal([ConsoleType.ALL], smcError.ConsoleType);
        Assert.Equal(CodeFlavor.SMC, smcError.CodeFlavor);
        Assert.True(smcError.IsError);
        Assert.Equal("FATAL_V12", smcError.Name);
        Assert.Equal("V_12P0 not available / not detected / timeout. -> Check short on V_12P0, check powersupply, check V_EXT (R5R20, R5R19), check PSU_V12P0_EN (R9A2, R9B2)", smcError.Description);
    }

    [Fact]
    public void ParsePostCodes_ShouldParseSPBootSuccess()
    {
        // Arrange
        var csvContent = File.ReadAllText(_testDataPath);

        // Act
        var result = CsvParsingService.ParsePostCodes(csvContent);
        var spSuccess = result.FirstOrDefault(x => x.CodeFlavor == CodeFlavor.SP && x.Name == "BOOT_SUCCESS");

        // Assert
        Assert.NotNull(spSuccess);
        Assert.Equal([ConsoleType.ALL], spSuccess.ConsoleType);
        Assert.Equal(CodeFlavor.SP, spSuccess.CodeFlavor);
        Assert.False(spSuccess.IsError);
        Assert.Equal("BOOT_SUCCESS", spSuccess.Name);
        Assert.Equal("", spSuccess.Description);
    }

    [Fact]
    public void ParsePostCodes_ShouldParseCPUBootSuccess()
    {
        // Arrange
        var csvContent = File.ReadAllText(_testDataPath);

        // Act
        var result = CsvParsingService.ParsePostCodes(csvContent);
        var cpuSuccess = result.FirstOrDefault(x => x.CodeFlavor == CodeFlavor.CPU && x.Name == "BOOT_SUCCESS");

        // Assert
        Assert.NotNull(cpuSuccess);
        Assert.Equal([ConsoleType.ALL], cpuSuccess.ConsoleType);
        Assert.Equal(CodeFlavor.CPU, cpuSuccess.CodeFlavor);
        Assert.False(cpuSuccess.IsError);
        Assert.Equal("BOOT_SUCCESS", cpuSuccess.Name);
        Assert.Equal("", cpuSuccess.Description);
    }

    [Fact]
    public void ParsePostCodes_ShouldHandleEmptyDescription()
    {
        // Arrange
        var csvContent = File.ReadAllText(_testDataPath);

        // Act
        var result = CsvParsingService.ParsePostCodes(csvContent);
        var emptyDesc = result.FirstOrDefault(x => x.Name == "FATAL_SBPOWERUP2");

        // Assert
        Assert.NotNull(emptyDesc);
        Assert.Equal("", emptyDesc.Description);
    }
}