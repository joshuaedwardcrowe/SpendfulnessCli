using YnabCli.Instructions.Arguments;

namespace YnabCli.Instructions.Builders;

public class GuidInstructionArgumentBuilder : NoDefaultInstructionArgumentBuilder, IInstructionArgumentBuilder
{
    public bool For(string? argumentValue) => Guid.TryParse(argumentValue, out _);

    public InstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        var parsedArgumentValue = Guid.Parse(validArgumentValue);
        return new TypedInstructionArgument<Guid>(argumentName, parsedArgumentValue);
    }
}