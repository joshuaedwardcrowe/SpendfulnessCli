namespace Cli.Instructions.Builders;

// TODO: Write unit tests.
internal abstract class NoDefaultInstructionArgumentBuilder
{
    protected TValueType GetValidValue<TValueType>(string argumentName, TValueType? argumentValue) where TValueType : notnull
    {
        if (argumentValue == null)
        {
            throw new ArgumentNullException($"Argument {argumentName} cannot be null");
        }

        return argumentValue;
    }
}