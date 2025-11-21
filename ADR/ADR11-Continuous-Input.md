# Continuous Input

## Premise

A command-line interface needs to maintain an interactive session where users can enter multiple commands sequentially without restarting the application. The CLI should continuously accept input, process commands, display results, and wait for the next input until the user explicitly exits the session.

The system needs to:
- Maintain a persistent session throughout multiple command executions
- Accept user input repeatedly without application restarts
- Execute each command independently while sharing session state
- Allow the user to exit gracefully when desired
- Support lifecycle hooks for customizing behavior at key interaction points

## Problem

Building a continuous input loop for CLI applications presents several challenges:

1. **Session Persistence** - The application needs to remain running between command executions while managing state appropriately. Each command execution should be independent, but the session context should persist.

2. **Input/Output Coordination** - The system must coordinate the timing of prompts, command execution, and result display to provide a smooth user experience without race conditions or awkward pauses.

3. **Lifecycle Extensibility** - Different CLI implementations may need to perform custom actions at various points in the session lifecycle (startup, before/after commands, shutdown) without modifying core loop logic.

4. **Clean Exit Strategy** - The loop needs a reliable mechanism to terminate gracefully without abrupt interruption or leaving resources in an inconsistent state.

5. **State Management** - The continuous loop must work with the workflow state machine (see ADR09) to coordinate command execution while maintaining clear boundaries between session-level and command-level concerns.

6. **Asynchronous Command Handling** - Commands may perform I/O or long-running operations. The loop must handle asynchronous execution without blocking the user interface or creating confusing states.

## Solution

The Continuous Input concept is implemented in the `CliApp` abstract base class, which provides a template for building interactive CLI applications with a persistent input loop.

### Architecture Overview

```
CliApp (Session Loop)
    ├── Uses → ICliWorkflow (manages command execution state)
    ├── Uses → ICliCommandOutcomeIo (handles user interaction)
    └── Provides → Lifecycle Hooks (extensibility points)
        ├── OnSessionStart()
        ├── OnRunCreated()
        ├── OnRunStarted()
        ├── OnRunComplete()
        └── OnSessionEnd()
```

### 1. The CliApp Base Class

`CliApp` implements the continuous input loop as a template method pattern:

```csharp
public abstract class CliApp
{
    private readonly ICliWorkflow _workflow;
    protected readonly ICliCommandOutcomeIo Io;

    protected CliApp(ICliWorkflow workflow, ICliCommandOutcomeIo io)
    {
        _workflow = workflow;
        Io = io;
    }
    
    public async Task Run()
    { 
        OnSessionStart();
        
        while (_workflow.Status != CliWorkflowStatus.Stopped)
        {
            var run = _workflow.NextRun();
            
            OnRunCreated(run);
            
            var ask = Io.Ask();
            
            var runTask = run.RespondToAsk(ask);
            
            OnRunStarted(run, ask);

            var outcomes = await runTask;
            
            Io.Say(outcomes);
            
            OnRunComplete(run, outcomes);
        }
        
        OnSessionEnd(_workflow.Runs);
    }

    protected virtual void OnSessionStart() { }
    protected virtual void OnRunCreated(CliWorkflowRun run) { }
    protected virtual void OnRunStarted(CliWorkflowRun run, string? ask) { }
    protected virtual void OnRunComplete(CliWorkflowRun run, CliCommandOutcome[] outcomes) { }
    protected virtual void OnSessionEnd(List<CliWorkflowRun> runs) { }
}
```

**Key Design Decision**: The base class provides the complete loop implementation with empty virtual hooks. This ensures consistency across all CLI implementations while allowing each to customize behavior at specific points without duplicating the core loop logic.

### 2. The Continuous Loop Pattern

The `Run()` method implements a Read-Execute-Display Loop (REDL) that continues until explicitly stopped:

**Execution Flow**:

1. **Session Initialization** (`OnSessionStart`)
   - Called once before entering the loop
   - Initialize session resources, display welcome message
   
2. **Enter Loop** (condition: `_workflow.Status != CliWorkflowStatus.Stopped`)
   - Loop continues indefinitely until workflow is explicitly stopped
   
3. **Prepare Run** (`_workflow.NextRun()`)
   - Create or reuse a workflow run for command execution
   - Run is either newly created or reused if previous run achieved reusable outcome
   
4. **Run Creation Hook** (`OnRunCreated`)
   - Notify that run is ready
   - Display prompt or prepare UI for input
   
5. **Get Input** (`Io.Ask()`)
   - Blocks waiting for user to enter command text
   - Returns `string?` which may be null or empty
   
6. **Start Execution** (`run.RespondToAsk(ask)`)
   - Begin command processing asynchronously
   - Returns Task that will complete when command finishes
   
7. **Execution Started Hook** (`OnRunStarted`)
   - Called immediately after starting command processing
   - Command runs concurrently while this hook executes
   - Use for progress indicators, logging, or concurrent operations
   
8. **Await Result** (`await runTask`)
   - Wait for command processing to complete
   - Receives array of command outcomes
   
9. **Display Results** (`Io.Say(outcomes)`)
   - Show command results to user
   - Formats outcomes appropriately for display
   
10. **Completion Hook** (`OnRunComplete`)
    - Called after command finishes and results are displayed
    - Perform cleanup, save state, display statistics
    
11. **Repeat** - Return to step 2 unless workflow status is `Stopped`

12. **Session Termination** (`OnSessionEnd`)
    - Called once after exiting the loop
    - Display summary, cleanup resources, persist final state

**Key Design Decision**: Command execution (`RespondToAsk`) is started before the `OnRunStarted` hook is called. This allows the hook to execute concurrently with command processing, enabling responsive UI updates like progress indicators without blocking command execution.

### 3. Integration with Workflow State Machine

The continuous input loop delegates command lifecycle management to the `ICliWorkflow` (see ADR09):

- **`NextRun()`** - Returns a `CliWorkflowRun` for executing the next command
  - May create a new run or reuse an existing one that achieved reusable outcome
  - Each run manages its own state machine (NotInitialized → Created → Running → Finished)
  
- **`Status`** - Tracks workflow status (Started/Stopped)
  - Loop continues while status is `Started`
  - Calling `workflow.Stop()` sets status to `Stopped`, terminating the loop
  
- **`Runs`** - Collection of all executed workflow runs
  - Provides command history for session summary
  - Enables tracking statistics like execution count and total time

**Separation of Concerns**:
- `CliApp` manages the continuous input loop and user interaction
- `CliWorkflow` manages command execution state and routing
- `CliWorkflowRun` manages individual command lifecycle

### 4. I/O Abstraction

The loop uses `ICliCommandOutcomeIo` for all user interaction:

- **`Ask()`** - Retrieve input from user
  - Blocks until input is received
  - Returns `string?` (may be null/empty)
  - Abstraction enables testing, mocking, alternative input sources
  
- **`Say(outcomes)`** - Display results to user
  - Takes array of `CliCommandOutcome` objects
  - Implementation determines formatting (console, file, network)
  - Abstraction enables alternative output destinations

**Key Design Decision**: Using abstractions instead of direct `Console.ReadLine()` and `Console.WriteLine()` enables unit testing, alternative I/O implementations, and separation of presentation concerns from business logic.

### 5. Lifecycle Hooks

Five virtual methods allow derived classes to customize behavior:

**OnSessionStart()**
- Execute once before loop begins
- Use case: Welcome message, resource initialization, configuration display
- Example:
  ```csharp
  protected override void OnSessionStart()
  {
      Io.Say($"Welcome to Spendfulness CLI!");
      Io.Pause();
  }
  ```

**OnRunCreated(CliWorkflowRun run)**
- Execute before each command, after run creation
- Use case: Display prompt, reset state, prepare UI
- Example:
  ```csharp
  protected override void OnRunCreated(CliWorkflowRun workflowRun)
  {
      Io.Say($"Please enter a command:");
      Io.Pause();
  }
  ```

**OnRunStarted(CliWorkflowRun run, string? ask)**
- Execute after command processing begins
- Runs concurrently with command execution
- Use case: Progress indicators, logging, tracking
- Example:
  ```csharp
  protected override void OnRunStarted(CliWorkflowRun workflowRun, string? ask)
  {
      Io.Say($"Executing command: {ask}");
      Io.Pause();
  }
  ```

**OnRunComplete(CliWorkflowRun run, CliCommandOutcome[] outcomes)**
- Execute after command finishes and results are displayed
- Use case: Statistics display, state persistence, cleanup
- Example:
  ```csharp
  protected override void OnRunComplete(CliWorkflowRun run, CliCommandOutcome[] outcomes)
  {
      var states = run.State.Changes.Select(change => change.To.ToString()).ToList();
      var timeline = string.Join(", ", states);
      Io.Say($"Timeline: {timeline} in {run.State.Stopwatch.Elapsed.Seconds}s");
      
      var changes = _dbContext.ChangeTracker.Entries()
          .Where(x => x.State != EntityState.Unchanged);
      _dbContext.SaveChanges();
      Io.Say($"Saved {changes.Count()} changes.");
  }
  ```

**OnSessionEnd(List<CliWorkflowRun> runs)**
- Execute once after loop exits
- Use case: Summary statistics, final cleanup, goodbye message
- Example:
  ```csharp
  protected override void OnSessionEnd(List<CliWorkflowRun> runs)
  {
      Io.Say($"CLI runs executed: {runs.Count}");
      
      var totalTime = runs
          .Select(run => run.State.Stopwatch.Elapsed)
          .Aggregate(TimeSpan.Zero, (current, elapsed) => current + elapsed);
      Io.Say($"Total time: {totalTime.Seconds}s");
  }
  ```

**Key Design Decision**: All hooks are optional with empty default implementations. This follows the principle of least surprise—derived classes only override hooks they need, without being forced to implement irrelevant methods.

### 6. Exit Strategy

The loop terminates when `_workflow.Status` becomes `CliWorkflowStatus.Stopped`. This requires:

1. A command that calls `_workflow.Stop()` (typically `/exit` or `/quit`)
2. The command completes its current execution
3. The loop checks status and exits instead of repeating
4. `OnSessionEnd` hook is called for final cleanup

**Key Design Decision**: Exit is cooperative rather than immediate. The current command completes before the loop exits. This ensures data integrity and allows proper cleanup. However, it means long-running commands cannot be interrupted through this mechanism.

## Constraints

### Single-Threaded Command Execution

Commands execute sequentially, one at a time. While individual commands may be asynchronous, the loop waits for each to complete before accepting new input. Concurrent command execution would require a fundamentally different architecture with command queues and coordination mechanisms.

### Blocking Input

The `Io.Ask()` call blocks the loop until input is received. The CLI cannot perform background work or respond to events while waiting for input. Alternative input mechanisms (async, event-driven) would require changing the I/O abstraction.

### No Built-in Cancellation

Once a command starts executing, it cannot be cancelled through the continuous input loop. The loop must wait for completion or exception. Adding cancellation would require:
- Threading cancellation tokens through the entire pipeline
- Cancellation commands or keyboard shortcuts
- Graceful command termination logic

### Workflow Coupling

The loop is tightly coupled to `ICliWorkflow`. The loop structure assumes:
- Workflow provides runs on demand
- Workflow status controls loop termination
- Workflow manages command routing and execution

Changing workflow semantics would impact the loop implementation.

### Fixed Hook Points

The five lifecycle hooks execute at predetermined points in the loop. Implementations cannot:
- Add additional hooks without modifying the base class
- Change hook execution order
- Skip or conditionally execute hooks

If different extension points are needed, consider:
- Using existing hooks creatively
- Wrapping the I/O abstraction
- Extending the workflow instead

### Memory Retention

The workflow keeps all runs in memory for the session duration. For long sessions with many commands, this could consume significant memory. The `Runs` collection grows unbounded and is only used for statistics in `OnSessionEnd`.

### No Command History Navigation

The loop doesn't provide built-in command history (up/down arrow to recall previous commands). This is typically a terminal feature, but CLIs built on this pattern don't implement it at the application level.

### Synchronous Hooks

All lifecycle hooks are synchronous (`void` methods). Implementations needing async operations in hooks must:
- Block synchronously (not ideal)
- Fire and forget async operations (risky)
- Perform async work in command handlers instead (recommended)

## Questions & Answers

### Why is this called "Continuous Input" instead of REPL or REDL?

"Continuous Input" emphasizes the persistent session aspect—the application continuously accepts commands without restarting. While similar to REPL (Read-Eval-Print-Loop), this pattern is more specific to command-line applications with workflow state machines and lifecycle hooks.

### What happens if the user provides no input or invalid input?

The `Io.Ask()` method may return null or empty string. The workflow's `RespondToAsk()` method validates input and returns `CliCommandNothingOutcome` for invalid input, which is displayed to the user. The loop continues normally.

### Can the loop be paused and resumed?

Not with the current design. The workflow status is either `Started` or `Stopped`. Adding pause/resume functionality would require:
- Additional workflow states
- State transition validation
- Decision on whether to pause mid-command or between commands

### Why pass outcomes array instead of single outcome?

Commands may produce multiple outcomes (e.g., a table and a message). The array allows commands to return multiple results that are displayed together. The I/O layer handles formatting multiple outcomes appropriately.

### How do you stop an infinite loop if workflow.Stop() is never called?

This is by design. The application developer is responsible for implementing an exit command (e.g., `/exit`, `/quit`) that calls `workflow.Stop()`. Without such a command, users must forcibly terminate the process (Ctrl+C). This is consistent with other REPL applications.

### What if a command throws an unhandled exception?

The workflow's `RespondToAsk()` method has exception handling (see ADR09) that converts exceptions to `CliCommandExceptionOutcome` objects. These are displayed to the user, and the loop continues. The continuous input loop itself doesn't have explicit exception handling, relying on the workflow layer.

### Can multiple commands be chained or piped?

No. Each loop iteration processes one command from start to finish. Command chaining or pipes would require:
- Enhanced instruction parser to detect chains
- Workflow support for command pipelines
- Mechanism to pass output from one command as input to another

### Why is OnRunStarted called before awaiting the command task?

This allows the hook to execute concurrently with command processing. Implementations can display progress indicators, show loading animations, or perform other tasks while the command runs. The alternative (calling after await) would make the hook pointless since the command would already be finished.

### Can I implement my own main loop instead of using CliApp?

Yes, `CliApp` is just a convenient base class. You can implement your own loop using `ICliWorkflow` and `ICliCommandOutcomeIo` directly. However, you'd lose the lifecycle hooks and would need to implement similar extensibility mechanisms yourself.

### How do I add custom behavior between runs?

Use the `OnRunComplete` and `OnRunCreated` hooks. These execute at the boundary between command executions, providing a natural point to reset state, save data, or display prompts.

### What is the purpose of NextRun() returning a reusable run?

Some commands produce reusable outcomes that should apply to subsequent commands (see ADR10). `NextRun()` returns the last run that achieved reusable outcome, allowing the next command to build upon it. This enables command composition patterns.

### Why is the workflow passed to the constructor instead of being created internally?

Dependency injection. The workflow is created by the DI container with all its dependencies. This enables:
- Testing with mock workflows
- Different workflow implementations
- Flexible configuration without modifying CliApp

### Can I nest CLI applications or run one inside another?

Not practically. Each `CliApp` maintains its own workflow and runs its own loop. Nesting would require complex coordination of I/O and state. For sub-commands or nested interactions, implement them as commands within the workflow rather than separate CLI applications.
