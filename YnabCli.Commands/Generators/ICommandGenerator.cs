using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Generators;

public interface ICommandGenerator
{
    // TODO: I don't like that this is an ICommand. I'd prefer specific impls.
    ICommand Generate(string? subCommandName, List<InstructionArgument> arguments);
}