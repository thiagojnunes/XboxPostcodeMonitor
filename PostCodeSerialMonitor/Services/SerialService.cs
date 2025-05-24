using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PostCodeSerialMonitor.Services;
public class SerialService : IDisposable
{
    private readonly ILogger<SerialService> _logger;
    private ISerialPort? _serialPort;
    private CancellationTokenSource? _readCts;
    public event Action<string>? DataReceived;
    public event Action? Disconnected;
    public event Action? DeviceStateChanged;
    public event Action? DeviceConfigChanged;

    public string FirmwareVersion { get; private set; } = string.Empty;
    public string BuildDate { get; private set; } = string.Empty;

    public bool MirrorDisplay { get; private set; }
    public bool PortraitMode { get; private set; }
    public bool PrintTimestamps { get; private set; }
    public bool PrintColors { get; private set; }

    public SerialService(ILogger<SerialService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<string> GetPortNames()
    {
        return SerialPort.GetPortNames();
    }

    public bool IsOpen => _serialPort?.IsOpen ?? false;

    public async Task GoToREPL()
    {
        if (!IsOpen)
            return;

        // Reset sequence
        for (int attempt = 0; attempt < 2; attempt++)
        {
            _serialPort?.WriteLine("\x03"); // CTRL+C
            _serialPort?.WriteLine(""); // ENTER

            await ReadUntilPrompt();
        }
    }

    public async Task ChangeConfigAndContinue(string command)
    {
        if (!IsOpen)
            return;

        _readCts?.Cancel();

        await GoToREPL();
        _serialPort?.WriteLine(command);

        // Update config state
        _serialPort?.WriteLine("config");
        await ParseConfigState();

        _serialPort?.WriteLine("post");

        _readCts = new CancellationTokenSource();

        // Resume reading loop
        _ = Task.Run(() => ReadLoop(_readCts.Token));
    }

    public async Task ConnectAsync(string portName, int baudRate = 115200)
    {
        if (_serialPort != null && _serialPort.IsOpen)
            throw new InvalidOperationException("Serial port already open");

        _serialPort = new SerialPortWrapper(portName, baudRate);
        _serialPort.Open();
        _readCts = new CancellationTokenSource();

        await GoToREPL();

        // Get version info
        _serialPort.WriteLine("version");
        _serialPort.WriteLine("");
        await ParseVersionInfo();

        // Get config state
        _serialPort.WriteLine("config");
        _serialPort.WriteLine("");
        await ParseConfigState();

        if (PrintColors)
        {
            _serialPort.WriteLine("colors");
            _serialPort.WriteLine("");
        }

        // Start post command
        _serialPort.WriteLine("post");

        // Start reading loop
        _ = Task.Run(() => ReadLoop(_readCts.Token));
    }

    private async Task<string> ReadUntilPrompt()
    {
        return await Task.Run(() =>
        {
            var output = "";

            while (_serialPort!.BytesToRead > 0)
            {
                output += _serialPort!.ReadChar();
            }

            if (!output.EndsWith(">> "))
            {
                _logger.LogWarning("Got no prompt in the end, Content: {content}", output);
            }
            return output;
        });
    }

    private async Task ParseVersionInfo()
    {
        var res = await ReadUntilPrompt();
        if (!res.Contains("FW: "))
        {
            Console.WriteLine("Failed to get version reply!");
            return;
        }

        foreach (var line in res.Split("\r\n"))
        {
            if (line.StartsWith("FW: "))
            {
                var parts = line.Substring(4).Split(' ');
                if (parts.Length >= 2)
                {
                    FirmwareVersion = parts[0];
                    BuildDate = parts[1];
                    DeviceStateChanged?.Invoke();
                }
            }
        }
    }

    private async Task ParseConfigState()
    {
        var res = await ReadUntilPrompt();
        if (!res.Contains("Display mirrored:"))
        {
            Console.WriteLine("Failed to get config state!");
            return;
        }

        foreach (var line in res.Split("\r\n"))
        {
            if (line.Contains("Display mirrored:"))
                MirrorDisplay = line.Contains("ON") ? true : false;
            else if (line.Contains("Disp rotation portrait:"))
                PortraitMode = line.Contains("ON") ? true : false;
            else if (line.Contains("Print timestamps:"))
                PrintTimestamps = line.Contains("ON") ? true : false;
            else if (line.Contains("Print colors:"))
                PrintColors = line.Contains("ON");
        }

        DeviceConfigChanged?.Invoke();
    }

    public void Disconnect()
    {
        if (_serialPort != null)
        {
            _readCts?.Cancel();
            _serialPort.Close();
            _serialPort.Dispose();
            _serialPort = null;
            Disconnected?.Invoke();
        }
        MirrorDisplay = false;
        PortraitMode = false;
        PrintTimestamps = false;
        PrintColors = false;
    }

    private void ReadLoop(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested && _serialPort != null && _serialPort.IsOpen)
            {
                var line = _serialPort.ReadLine();
                DataReceived?.Invoke(line);
            }
        }
        catch (Exception)
        {
            Disconnected?.Invoke();
        }
    }

    public void Dispose()
    {
        Disconnect();
    }
}