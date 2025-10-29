using Cli;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Workflow;
using Spendfulness.Database;

namespace Spendfulness.Cli;

public class SpendfulnessCli : OriginalCli
{
    private readonly SpendfulnessDbContext _spendfulnessDbContext;
    
    public SpendfulnessCli(CliWorkflow workflow, CliCommandOutcomeIo io, SpendfulnessDbContext spendfulnessDbContext)
        : base(workflow, io)
    {
        _spendfulnessDbContext = spendfulnessDbContext;
    }

    protected override void OnSessionStart()
    {
        Io.Say($"Welcome to Spendfulness CLI!");
        Io.Pause();
    }

    protected override void OnRunCreated(CliWorkflowRun workflowRun)
    {
        Io.Say($"Please enter a command:");
        Io.Pause();
    }

    protected override void OnRunStarted(CliWorkflowRun workflowRun, string ask)
    {
        Io.Say($"Executing command: {ask}");
        Io.Pause();
    }

    protected override void OnRunComplete(CliWorkflowRun run, CliCommandOutcome outcome)
    {
        Io.Pause();
        
        Io.Say($"Command outcome was: {outcome.Kind}");

        var states = run.State.Changes
            .Select(change => change.MovedTo.ToString())
            .ToList();
        
        var timeline = string.Join(", ", states);
        
        Io.Say($"Timeline: {timeline} in {run.State.Stopwatch.Elapsed.Seconds}s");
        
        // Using synchronous because you cant run commands in parallel.
        var changes = _spendfulnessDbContext.ChangeTracker.Entries();
        
        _spendfulnessDbContext.SaveChanges();
        Io.Say($"Saved {changes.Count()} changes.");
        
        Io.Pause();
    }

    protected override void OnSessionEnd(List<CliWorkflowRun> runs)
    {
        Io.Pause();
        
        Io.Say($"CLI session complete");
        Io.Say($"CLI runs executed: {runs.Count}");
        
        var totalTime = runs
            .Select(run => run.State.Stopwatch.Elapsed)
            .Aggregate(TimeSpan.Zero, (current, elapsed) => current + elapsed);
        
        Io.Say($"Total time: {totalTime.Seconds}s");
        
        Io.Pause();
    }
}