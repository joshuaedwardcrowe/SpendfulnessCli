using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using SpendfulnessCli.Commands.Personalisation.Users.Active;
using SpendfulnessCli.Commands.Personalisation.Users.Create;
using SpendfulnessCli.Commands.Personalisation.Users.Switch;

namespace SpendfulnessCli.Commands.Personalisation.Users;

public class UserGenericCliCommandGenerator : ICliCommandGenerator<UserCliCommand>
{
    public CliCommand Generate(CliInstruction instruction) 
        => instruction.SubInstructionName switch
        {
            UserCliCommand.SubCommandNames.Create => GenerateCreateCommand(instruction.Arguments),
            UserCliCommand.SubCommandNames.Switch => GenerateSwitchCommand(instruction.Arguments),
            UserCliCommand.SubCommandNames.Active => new UserActiveCliCommand(),
            _ => new UserCliCommand()
        };

    private UserCreateCliCommand GenerateCreateCommand(List<CliInstructionArgument> arguments)
    {
        var userNameArgument = arguments.OfRequiredType<string>(UserCreateCliCommand.ArugmentNames.UserName);

        return new UserCreateCliCommand
        {
            UserName = userNameArgument.ArgumentValue
        };
    }

    private UserSwitchCliCommand GenerateSwitchCommand(List<CliInstructionArgument> arguments)
    {
        var userNameArgument = arguments.OfRequiredType<string>(UserCreateCliCommand.ArugmentNames.UserName);

        return new UserSwitchCliCommand
        {
            UserName = userNameArgument.ArgumentValue
        };
    }
}