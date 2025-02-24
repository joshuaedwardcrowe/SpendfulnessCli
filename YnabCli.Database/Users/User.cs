namespace YnabCli.Database.Users;

public class User
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool Active { get; set; }
}