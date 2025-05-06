using System;
using System.IO;
using System.Text.RegularExpressions;

class LogStandardizer
{
    static void Main(string[] args)
    {
        string inputPath = "input.txt";
        string outputPath = "output.txt";
        string problemsPath = "problems.txt";

        File.WriteAllText(outputPath, "");
        File.WriteAllText(problemsPath, "");

        foreach (var line in File.ReadLines(inputPath))
        {
            string standardized = ParseFormat1(line) ?? ParseFormat2(line);

            if (standardized != null)
                File.AppendAllText(outputPath, standardized + Environment.NewLine);
            else
                File.AppendAllText(problemsPath, line + Environment.NewLine);
        }

        Console.WriteLine("Обработка завершена.");
    }

    static string ParseFormat1(string line)
    {
        var pattern = @"^(\d{2}\.\d{2}\.\d{4}) (\d{2}:\d{2}:\d{2}\.\d+)\s+(INFORMATION|WARNING|ERROR|DEBUG)\s+(.+)$";
        var match = Regex.Match(line, pattern);
        if (!match.Success) return null;

        string date = DateTime.ParseExact(match.Groups[1].Value, "dd.MM.yyyy", null).ToString("yyyy-MM-dd");
        string time = match.Groups[2].Value;
        string level = NormalizeLevel(match.Groups[3].Value);
        string message = match.Groups[4].Value;

        return $"{date}\t{time}\t{level}\tDEFAULT\t{message}";
    }

    static string ParseFormat2(string line)
    {
        var pattern = @"^(\d{4}-\d{2}-\d{2}) (\d{2}:\d{2}:\d{2}\.\d+)\|\s*(INFO|WARN|ERROR|DEBUG)\|\d+\|([^\|]+)\|\s+(.+)$";
        var match = Regex.Match(line, pattern);
        if (!match.Success) return null;

        string date = match.Groups[1].Value;
        string time = match.Groups[2].Value;
        string level = NormalizeLevel(match.Groups[3].Value);
        string method = match.Groups[4].Value;
        string message = match.Groups[5].Value;

        return $"{date}\t{time}\t{level}\t{method}\t{message}";
    }

    static string NormalizeLevel(string level)
    {
        switch (level.ToUpperInvariant())
        {
            case "INFORMATION":
            case "INFO":
                return "INFO";
            case "WARNING":
            case "WARN":
                return "WARN";
            case "ERROR":
                return "ERROR";
            case "DEBUG":
                return "DEBUG";
            default:
                return "UNKNOWN"; 
        }
    }
}
