namespace YnabProgressConsole;

public class ConsoleInput
{
    public string CommandName { get; set; }
    public List<string> Arguments { get; set; }
    
    public ConsoleInput(string input)
    {
        var inputTokens = input.Split(' ');
        var commandToken = inputTokens[0];
        var commandName = commandToken.Substring(1);
        var argumentTokens = inputTokens.Skip(1).ToList();
        
        CommandName = commandName;
        Arguments = argumentTokens;
    }
}