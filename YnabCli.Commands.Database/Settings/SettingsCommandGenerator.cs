using YnabCli.Commands.Database.Settings.Create;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Database.Settings;

public class SettingsCommandGenerator : ICommandGenerator, ITypedCommandGenerator<SettingsCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        if (subCommandName == SettingsCommand.SubCommandNames.Create)
        {
            return GenerateCreateCommand(arguments);
        }

        return new SettingsCommand();
    }

    private SettingCreateCommand GenerateCreateCommand(List<InstructionArgument> arguments)
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