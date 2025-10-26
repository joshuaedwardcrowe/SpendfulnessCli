using System.Runtime.InteropServices.ComTypes;
using Cli.Commands.Abstractions.Io;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Workflow;
using Spendfulness.Database;

namespace Cli.Spendfulness;

public class SpendfulnessCli : OriginalCli
{
    private readonly SpendfulnessDbContext _spendfulnessDbContext;
    
    public SpendfulnessCli(CliWorkflow workflow, CliCommandOutcomeIo io, SpendfulnessDbContext spendfulnessDbContext)
        : base(workflow, io)
    {
        _spendfulnessDbContext = spendfulnessDbContext;
    }

    protected override void OnRun(CliWorkflow workflow, CliIo io)
    {
        io.Say($"New world CLI started");
    }

    protected override void OnRunCreated(CliWorkflowRun workflowRun, CliIo io)
    {
        io.Say($"New world CLI run created");
    }

    protected override void OnRunStarted(CliWorkflowRun workflowRun, CliIo io)
    {
        io.Say($"New world CLI run started");
    }

    protected override void OnRunComplete(CliCommandOutcome cliCommandOutcome, CliWorkflowRun workflowRun, CliIo io)
    {
        io.Pause();
        
        io.Say($"Command outcome was: {cliCommandOutcome.Kind}");

        var states = workflowRun.GetTimeline();
        var timeline = string.Join(", ", states);
        
        io.Say($"Timeline: {timeline}");
        
        // Using synchronous because you cant run commands in parallel.
        var changes = _spendfulnessDbContext.ChangeTracker.Entries();
        
        _spendfulnessDbContext.SaveChanges();
        io.Say($"Saved {changes.Count()} changes.");
    }
}