namespace Ynab.Clients;

public class ApiRequestLog
{
    public required string Method { get; set; }
    public required string Path { get; set; }
    public required int RemainingRequestAllowance { get; set; }
    public required long ResponeTime { get; set; }
}