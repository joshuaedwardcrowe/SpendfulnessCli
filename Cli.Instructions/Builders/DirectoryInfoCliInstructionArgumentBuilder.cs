using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Instructions.Builders;

internal class DirectoryInfoCliInstructionArgumentBuilder : ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue)
    {
        if (string.IsNullOrEmpty(argumentValue))
        {
            return false;
        }

        var hasNoInvalidChars = HasInvalidCharacters(argumentValue);
        var isFilePath = IsFilePath(argumentValue);

        return hasNoInvalidChars && isFilePath;
    }

    public CliInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var directoryInfo = new DirectoryInfo(argumentValue ?? string.Empty);
        
        return new ValuedCliInstructionArgument<DirectoryInfo>(argumentName, directoryInfo);
    }
    
    private static bool HasInvalidCharacters(string path)
    {
        var invalidPathChars = Path.GetInvalidPathChars();
        var indexOfInvalidChar = path.IndexOfAny(invalidPathChars);
        return indexOfInvalidChar >= 0;
    }
    
    private static bool IsFilePath(string argumentValue)
    {
        return Path.IsPathRooted(argumentValue) || argumentValue.StartsWith($".");
    }
}