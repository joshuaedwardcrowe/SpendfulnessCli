using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Settings.Create;

public record SettingCreateCliCommand : CliCommand
{
    public static class ArgumentNames
    {
        public const string Type = "type";
        public const string Value = "value";
    }
    
    public required string Type { get; set; }
    public required string Value { get; set; }
}