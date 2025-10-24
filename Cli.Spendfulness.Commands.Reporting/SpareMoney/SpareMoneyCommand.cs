using Cli.Commands.Abstractions;

namespace Cli.Ynab.Commands.Reporting.SpareMoney;

public class SpareMoneyCommand : ICommand
{
    public static class SubCommandNames
    {
        public const string Help = "help";
    }

    public static class ArgumentNames
    {
        public const string Add = "add";
        public const string Minus = "minus";
        public const string MinusSavings = "minus-savings";
    }
    
    public decimal? Add { get; set; }
    public decimal? Minus { get; set; }
    public bool? MinusSavings { get; set; }
}