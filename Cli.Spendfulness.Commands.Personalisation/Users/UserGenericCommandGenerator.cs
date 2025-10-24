using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Spendfulness.Commands.Personalisation.Users.Active;
using Cli.Spendfulness.Commands.Personalisation.Users.Create;
using Cli.Spendfulness.Commands.Personalisation.Users.Switch;

namespace Cli.Spendfulness.Commands.Personalisation.Users;

public class UserGenericCommandGenerator : ICommandGenerator<UserCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
    {
        return subCommandName switch
        {
            UserCommand.SubCommandNames.Create => GenerateCreateCommand(arguments),
            UserCommand.SubCommandNames.Switch => GenerateSwitchCommand(arguments),
            UserCommand.SubCommandNames.Active => new UserActiveCommand(),
            _ => new UserCommand()
        };
    }

    private UserCreateCommand GenerateCreateCommand(List<ConsoleInstructionArgument> arguments)
    {
        var userNameArgument = arguments.OfRequiredType<string>(UserCreateCommand.ArugmentNames.UserName);

        return new UserCreateCommand
        {
            UserName = userNameArgument.ArgumentValue
        };
    }

    private UserSwitchCommand GenerateSwitchCommand(List<ConsoleInstructionArgument> arguments)
    {
        var userNameArgument = arguments.OfRequiredType<string>(UserCreateCommand.ArugmentNames.UserName);

        return new UserSwitchCommand
        {
            UserName = userNameArgument.ArgumentValue
        };
    }
}