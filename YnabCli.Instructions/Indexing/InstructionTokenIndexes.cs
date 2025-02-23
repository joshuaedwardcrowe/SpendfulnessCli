namespace YnabCli.Instructions.Indexing;

public class InstructionTokenIndexes
{
    public bool PrefixTokenIndexed { get; set; }
    public int PrefixTokenStartIndex { get; set; }
    public int PrefixTokenEndIndex { get; set; }
    
    public bool NameTokenIndexed { get; set; }
    public int NameTokenStartIndex { get; set; }
    public int NameTokenEndIndex { get; set; }
    
    public bool SubNameTokenIndexed { get; set; }
    public int SubNameStartIndex { get; set; }
    public int SubNameEndIndex { get; set; }
    
    public bool ArgumentTokensIndexed { get; set; }
    public int ArgumentTokensStartIndex { get; set; }
    public int ArgumentTokensEndIndex { get; set; }
}