namespace YnabProgressConsole.Instructions;

public class InstructionTokens
{
    public required string NameToken { get; set; }
    public required IEnumerable<string> ArgumentTokens { get; set; }
}