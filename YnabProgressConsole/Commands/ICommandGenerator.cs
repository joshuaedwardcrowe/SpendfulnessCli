namespace YnabProgressConsole.Commands;

public interface ICommandGenerator
{
    ICommand Generate(List<string> arguments);
}