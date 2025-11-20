using Cli;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Workflow;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;

namespace SpendfulnessCli;

public class SpendfulnessCliApp : CliApp
{
    private readonly SpendfulnessDbContext _spendfulnessDbContext;
    
    public SpendfulnessCliApp(CliWorkflow workflow, CliCommandOutcomeIo io, SpendfulnessDbContext spendfulnessDbContext)
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

    protected override void OnRunComplete(CliWorkflowRun run, CliCommandOutcome[] outcomes)
    {
        Io.Pause();
        
        SayOutcomeTypeNames(outcomes);

        var states = run.State.Changes
            .Select(change => change.To.ToString())
            .ToList();
        
        var timeline = string.Join(", ", states);
        
        Io.Say($"Timeline: {timeline} in {run.State.Stopwatch.Elapsed.Seconds}s");
        
        // Using synchronous because you cant run commands in parallel.
        var beingTracked = _spendfulnessDbContext.ChangeTracker.Entries();

        var changes = beingTracked.Where(x => x.State != EntityState.Unchanged);
        
        _spendfulnessDbContext.SaveChanges();
        Io.Say($"Saved {changes.Count()} changes.");
        
        Io.Pause();
    }
    
    private void SayOutcomeTypeNames(CliCommandOutcome[] outcomes)
    {
        var outcomeTypeNames = outcomes
            .Select(o => o.GetType().Name)
            .ToList();
        
        var outcomeTypeList = string.Join(", ", outcomeTypeNames);
        
        Io.Say($"Command outcome was: {outcomeTypeList}");
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