namespace Spendfulness.Tools;

public static class DirectoryInfoExtensions
{
    public static string GetFilePath(this DirectoryInfo directoryInfo, string fileName, FileFormat fileFormat)
    {
        var directoryPath = directoryInfo.ToString();
        var format = fileFormat.ToString().ToLowerInvariant();
        var filePath = $"{fileName}-{DateTime.UtcNow:yyyyMMddHHmmss}.{format}";
        return Path.Combine(directoryPath, filePath);
    }
}