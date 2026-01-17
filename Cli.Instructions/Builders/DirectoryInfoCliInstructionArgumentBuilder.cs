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

        bool hasNoInvalidChars = argumentValue.IndexOfAny(Path.GetInvalidPathChars()) < 0;
        bool isFilePath = Path.IsPathRooted(argumentValue) || argumentValue.StartsWith(".");

        return hasNoInvalidChars && isFilePath;
    }

    public CliInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var directoryInfo = new DirectoryInfo(argumentValue ?? string.Empty);
        
        return new ValuedCliInstructionArgument<DirectoryInfo>(argumentName, directoryInfo);
    }
}