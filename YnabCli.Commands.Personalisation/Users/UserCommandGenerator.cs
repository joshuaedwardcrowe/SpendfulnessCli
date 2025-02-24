using YnabCli.Commands.Personalisation.Users.Active;
using YnabCli.Commands.Personalisation.Users.Create;
using YnabCli.Commands.Personalisation.Users.Switch;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Personalisation.Users;

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