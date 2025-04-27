namespace YnabCli.Database.SpendingSamples;

public class SpendingSampleGroup
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public string YnabTransactionDerivedFromId { get; set; }
    public ICollection<SpendingSample> Samples { get; set; }
}