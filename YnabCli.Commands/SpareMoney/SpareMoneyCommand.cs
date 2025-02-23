namespace YnabCli.Commands.SpareMoney;

public class SpareMoneyCommand : ICommand
{
    public const string CommandName = "spare-money";
    public const string ShorthandCommandName = "sm";

    public static class SubCommandNames
    {
        public const string Help = "help";
    }

    public static class ArgumentNames
    {
        public const string Minus = "minus";
        public const string MinusSavings = "minus-savings";
    }
    
    public decimal? Minus { get; set; }
    public bool? MinusSavings { get; set; }
}