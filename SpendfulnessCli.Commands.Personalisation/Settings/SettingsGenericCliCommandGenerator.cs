using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using SpendfulnessCli.Commands.Personalisation.Settings.Create;
using SpendfulnessCli.Commands.Personalisation.Settings.View;

namespace SpendfulnessCli.Commands.Personalisation.Settings;

public class SettingsGenericCliCommandGenerator : ICliCommandGenerator<SettingsCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
        => instruction.SubInstructionName switch
        {
            SettingsCliCommand.SubCommandNames.Create => GenerateCreateCommand(instruction.Arguments),
            SettingsCliCommand.SubCommandNames.View => new SettingsViewCliCommand(),
            _ => new SettingsCliCommand()
        };
    
    private SettingCreateCliCommand GenerateCreateCommand(List<CliInstructionArgument> arguments)
    {
        var nameArgument = arguments.OfRequiredType<string>(SettingCreateCliCommand.ArgumentNames.Type);
        var valueArgument = arguments.OfRequiredType<string>(SettingCreateCliCommand.ArgumentNames.Value);

        return new SettingCreateCliCommand
        {
            Type = nameArgument.ArgumentValue,
            Value = valueArgument.ArgumentValue,
        };
    }
}