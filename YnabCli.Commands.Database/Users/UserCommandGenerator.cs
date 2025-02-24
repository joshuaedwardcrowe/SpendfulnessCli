using YnabCli.Commands.Database.Users.Active;
using YnabCli.Commands.Database.Users.Create;
using YnabCli.Commands.Database.Users.Switch;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Database.Users;

public class UserCommandGenerator : ICommandGenerator, ITypedCommandGenerator<UserCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        return subCommandName switch
        {
            UserCommand.SubCommandNames.Create => GenerateCreateCommand(arguments),
            UserCommand.SubCommandNames.Switch => GenerateSwitchCommand(arguments),
            UserCommand.SubCommandNames.Active => new UserActiveCommand(),
            _ => new UserCommand()
        };
    }

    private UserCreateCommand GenerateCreateCommand(List<InstructionArgument> arguments)
    {
        var userNameArgument = arguments.OfRequiredType<string>(UserCreateCommand.ArugmentNames.UserName);

        return new UserCreateCommand
        {
            UserName = userNameArgument.ArgumentValue
        };
    }

    private UserSwitchCommand GenerateSwitchCommand(List<InstructionArgument> arguments)
    {
        var userNameArgument = arguments.OfRequiredType<string>(UserCreateCommand.ArugmentNames.UserName);

        return new UserSwitchCommand
        {
            UserName = userNameArgument.ArgumentValue
        };
    }
}