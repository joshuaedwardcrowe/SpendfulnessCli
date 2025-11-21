namespace Cli.Commands.Abstractions.Attributes;

public class FactoryFor(Type commandType) : Attribute
{
    public Type CommandType { get; } = commandType;
}