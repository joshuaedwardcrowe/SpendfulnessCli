using Cli.Commands.Abstractions.Extensions;
using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions;

public interface IUnidentifiedCliCommandGenerator
{
    CliCommand Generate(CliInstruction instruction);

    void NextCommands(CliNextCommmandDefinition commmandDefinition)
        => throw new NotImplementedException();
}

public class CliNextCommmandDefinition
{
    private readonly List<string> _nextCommands = new();
    
    
    public CliNextCommmandDefinition Next<TNextCommand>() where TNextCommand : CliCommand
    {
        var nextCommandName = typeof(TNextCommand)
            .GetCommandName()
            .ToLowerSplitString(CliInstructionConstants.DefaultCommandNameSeparator);

        _nextCommands.Add(nextCommandName);

        return this;
    }

    public bool CanMoveTo(string nextCommandNAme) => _nextCommands.Contains(nextCommandNAme);
}

