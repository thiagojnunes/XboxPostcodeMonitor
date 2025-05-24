using System;

namespace PostCodeSerialMonitor.Models;
// Simple model to hold log entry data
public class LogEntry
{
    public string RawText { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public DecodedCode? DecodedCode { get; set; }
    public string FormattedText => DecodedCode != null ? FormatDecodedText() : RawText;
    public bool IsWarning => SeverityLevel == CodeSeverity.Warning;
    public bool IsError => SeverityLevel == CodeSeverity.Error;
    public CodeSeverity SeverityLevel => DecodedCode?.SeverityLevel ?? CodeSeverity.Info;

    private string FormatDecodedText()
    {
        var details = new System.Text.StringBuilder();

        if (!string.IsNullOrEmpty(DecodedCode?.Name))
        {
            details.Append($"- {DecodedCode.Name}");
        }

        if (!string.IsNullOrEmpty(DecodedCode?.Description))
        {
            details.Append($"\n\t- {DecodedCode.Description}");
        }

        // Format flavor, index, and code with fixed spacing
        var formattedPrefix = $"{DecodedCode?.Flavor,-4} ({DecodedCode?.Index}): {DecodedCode?.Code,4:X4}";
        return $"{formattedPrefix}\t{details}";
    }
}