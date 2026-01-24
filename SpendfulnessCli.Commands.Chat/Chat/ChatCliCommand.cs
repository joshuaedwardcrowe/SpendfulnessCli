using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Chat.Chat;

public record ChatCliCommand(string Prompt) : CliCommand
{
    public static class ArgumentNames
    {
        public const string Prompt = "prompt";
    }
}