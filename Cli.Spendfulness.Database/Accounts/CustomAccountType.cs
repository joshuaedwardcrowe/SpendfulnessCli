using System.ComponentModel.DataAnnotations;

namespace Cli.Spendfulness.Database.Accounts;

public class CustomAccountType
{
    public int Id { get; set; }
    [MaxLength(2000)]
    public required string Name { get; set; }
}