using System;

namespace PostCodeSerialMonitor.Services;
public interface ISerialPort : IDisposable
{
    bool IsOpen { get; }
    int BytesToRead { get; }
    void Open();
    void Close();
    void Write(string text);
    void WriteLine(string text);
    char ReadChar();
    string ReadLine();
}