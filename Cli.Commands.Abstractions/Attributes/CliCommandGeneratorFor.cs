namespace Cli.Commands.Abstractions.Attributes;

public class CliCommandGeneratorFor(Type commandType) : Attribute
{
    public Type CommandType { get; } = commandType;
}