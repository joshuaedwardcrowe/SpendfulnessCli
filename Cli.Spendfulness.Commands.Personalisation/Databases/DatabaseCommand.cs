using Cli.Commands.Abstractions;

namespace Cli.Spendfulness.Commands.Personalisation.Databases;

public class DatabaseCommand : ICommand
{
    public static class SubCommandNames
    {
        public const string Create = "create";
    }
}