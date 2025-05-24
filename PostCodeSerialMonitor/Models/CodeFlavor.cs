using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace PostCodeSerialMonitor.Models;

public enum CodeFlavor
{
    UNKNOWN,
    SMC,
    SP,
    CPU,
    OS
}