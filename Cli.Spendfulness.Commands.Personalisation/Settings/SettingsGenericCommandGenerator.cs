using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Spendfulness.Commands.Personalisation.Settings.Create;
using Cli.Spendfulness.Commands.Personalisation.Settings.View;

namespace Cli.Spendfulness.Commands.Personalisation.Settings;

public class SettingsGenericCommandGenerator : ICommandGenerator<SettingsCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
    {
        return subCommandName switch
        {
            SettingsCommand.SubCommandNames.Create => GenerateCreateCommand(arguments),
            SettingsCommand.SubCommandNames.View => new SettingsViewCommand(),
            _ => new SettingsCommand()
        };
    }

    private SettingCreateCommand GenerateCreateCommand(List<ConsoleInstructionArgument> arguments)
    {
        var nameArgument = arguments.OfRequiredType<string>(SettingCreateCommand.ArgumentNames.Type);
        var valueArgument = arguments.OfRequiredType<string>(SettingCreateCommand.ArgumentNames.Value);

        return new SettingCreateCommand
        {
            Type = nameArgument.ArgumentValue,
            Value = valueArgument.ArgumentValue,
        };
    }
}