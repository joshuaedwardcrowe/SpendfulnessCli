namespace YnabCli.Commands.Database.Settings.Create;

public class SettingCreateCommand : ICommand
{
    public static class ArgumentNames
    {
        public const string Type = "type";
        public const string Value = "value";
    }
    
    public required string Type { get; set; }
    public required string Value { get; set; }
}