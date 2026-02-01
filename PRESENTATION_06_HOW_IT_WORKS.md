## 6. HOW IT WORKS
**Duration: 15 minutes**

### The Architecture - 7 Key Components

#### 1. Workflow - How Units of Work Are Moderated (2 min)

**What it is:** The session manager.

**Responsibilities:**
- Start/stop the CLI session
- Create new workflow runs for each command
- Maintain command history

```csharp
public class CliWorkflow
{
    public CliWorkflowStatus Status { get; set; } // Started | Stopped
    private List<CliWorkflowRun> _runs = [];
    
    public CliWorkflowRun NextRun()
    {
        var run = new CliWorkflowRun(_parser, _commandProvider, _mediator);
        _runs.Add(run);
        return run;
    }
    
    public void Stop() => Status = CliWorkflowStatus.Stopped;
}
```

**Key insight:** Workflow = session. Run = command.

#### 2. Runs - Units of Work (2 min)

**What it is:** A single command execution.

**Lifecycle:**
```
NotInitialized → Created → Running → Finished
                              ↓
                         [InvalidAsk | Exceptional]
```

**Code:**
```csharp
public class CliWorkflowRun
{
    public async Task<CliCommandOutcome[]> RespondToAsk(string? ask)
    {
        _state.ChangeTo(RunState.Created);
        
        if (string.IsNullOrEmpty(ask))
        {
            _state.ChangeTo(RunState.InvalidAsk);
            return [new CliCommandNothingOutcome()];
        }
        
        try
        {
            _state.ChangeTo(RunState.Running);
            var instruction = _parser.Parse(ask);
            var command = _provider.GetCommand(instruction);
            return await _mediator.Send(command);
        }
        catch (Exception ex)
        {
            _state.ChangeTo(RunState.Exceptional);
            return [new CliCommandExceptionOutcome(ex)];
        }
        finally
        {
            _state.ChangeTo(RunState.Finished);
        }
    }
}
```

**Key insight:** Each run is isolated. Errors don't crash the session.

#### 3. Operating Model - State Machine at Play (2 min)

**Why state machines?** Predictability and debuggability.

**Session States:**
```
Started → Stopped
```

**Run States:**
```
NotInitialized 
    → Created 
    → Running 
    → {InvalidAsk | Exceptional | Success} 
    → Finished
```

**State Validation:**
```csharp
private void ChangeTo(RunState newState)
{
    if (!IsValidTransition(_currentState, newState))
    {
        throw new InvalidStateTransitionException(
            $"Cannot transition from {_currentState} to {newState}"
        );
    }
    
    _currentState = newState;
    _recordedTransitions.Add(new StateTransition(DateTime.Now, newState));
}
```

**Benefits:**
- ✅ Impossible states are impossible
- ✅ Clear execution flow
- ✅ Easy to debug (see state history)
- ✅ Prevents bugs

#### 4. Instructions - Raw Input to Type-Safe Values (3 min)

**The problem:** User types `/deploy --env prod --replicas 3`

**We need:** Type-safe command object with `Environment="prod"` and `Replicas=3`

**The solution:** Three-stage pipeline.

**Stage 1: Token Indexer**
```
Input:  "/deploy --env prod --replicas 3"
Output: PrefixIndex=0, NameIndex=1-6, ArgIndex=8...
```
- Finds WHERE tokens are
- No parsing, just position detection

**Stage 2: Token Extractor**
```
Output: {
    Prefix: "/",
    Name: "deploy",
    Arguments: { 
        "env": "prod",
        "replicas": "3"
    }
}
```
- Extracts string values
- Uses indexes from Stage 1

**Stage 3: Argument Builders**
```csharp
public interface IConsoleInstructionArgumentBuilder
{
    bool For(string? value);  // Can I handle this?
    ConsoleInstructionArgument Create(string name, string? value);
}

// Concrete builders
public class IntArgumentBuilder : IConsoleInstructionArgumentBuilder
{
    public bool For(string? value) => int.TryParse(value, out _);
    
    public ConsoleInstructionArgument Create(string name, string? value)
    {
        return new TypedArgument<int>(name, int.Parse(value!));
    }
}
```

**Result:**
```csharp
public record ConsoleInstruction(
    string Name,
    IEnumerable<TypedConsoleInstructionArgument> Arguments
);
```

**Benefits:**
- ✅ Type-safe from input to handler
- ✅ Extensible (add new type builders)
- ✅ Compiler catches type errors
- ✅ IntelliSense works

#### 5. Building Commands - Instructions to DTOs (2 min)

**From instruction to command:**

```csharp
// Instruction (parsed input)
var instruction = new ConsoleInstruction(
    Name: "deploy",
    Arguments: [
        new TypedArgument<string>("env", "prod"),
        new TypedArgument<int>("replicas", 3)
    ]
);

// Command Generator
public class DeployCommandGenerator : ICliCommandGenerator
{
    public CliCommand Generate(ConsoleInstruction instruction)
    {
        var env = instruction.GetArgument<string>("env");
        var replicas = instruction.GetArgument<int>("replicas");
        
        return new DeployCommand(env, replicas);
    }
}

// Command (DTO)
public record DeployCommand(string Environment, int Replicas) : CliCommand;
```

**Why this step?** Separation of concerns:
- Parser doesn't know about commands
- Commands don't know about parsing
- Generators bridge the gap

#### 6. Routing - Sending Commands to Handlers (2 min)

**The routing mechanism:**

```csharp
// Registration (in DI)
services.AddKeyedTransient<ICliCommandGenerator>(
    "deploy",  // Instruction name
    (sp, key) => new DeployCommandGenerator()
);

// Routing (in command provider)
public CliCommand GetCommand(ConsoleInstruction instruction)
{
    var generator = _serviceProvider
        .GetKeyedService<ICliCommandGenerator>(instruction.Name);
        
    if (generator == null)
    {
        throw new NoCommandGeneratorException(
            $"No generator found for '{instruction.Name}'"
        );
    }
    
    return generator.Generate(instruction);
}

// Dispatch (via MediatR)
var outcome = await _mediator.Send(command);
```

**Benefits:**
- ✅ Add commands without touching routing code
- ✅ DI handles discovery
- ✅ MediatR handles dispatch
- ✅ Handlers are focused and testable

#### 7. Achieving Outcomes - Simpler Terminal Output (2 min)

**The outcome pattern:**

```csharp
// Base
public abstract class CliCommandOutcome { }

// Concrete outcomes
public class CliCommandOutputOutcome(string message) : CliCommandOutcome;
public class CliCommandTableOutcome(CliTable table) : CliCommandOutcome;
public class FilterCliCommandOutcome(CliListAggregatorFilter filter) : CliCommandOutcome;
public class CliCommandNothingOutcome : CliCommandOutcome;
public class CliCommandExceptionOutcome(Exception ex) : CliCommandOutcome;
```

**Handler returns outcomes:**
```csharp
public class DeployCommandHandler : IRequestHandler<DeployCommand, CliCommandOutcome[]>
{
    public async Task<CliCommandOutcome[]> Handle(DeployCommand cmd)
    {
        await _deployService.Deploy(cmd.Environment, cmd.Replicas);
        return OutcomeAs($"✅ Deployed to {cmd.Environment}");
    }
}
```

**I/O layer displays outcomes:**
```csharp
public void Say(CliCommandOutcome[] outcomes)
{
    foreach (var outcome in outcomes)
    {
        switch (outcome)
        {
            case CliCommandOutputOutcome output:
                Console.WriteLine(output.Message);
                break;
            case CliCommandTableOutcome table:
                Console.WriteLine(table.Table.ToMarkdownString());
                break;
            // ... other outcome types
        }
    }
}
```

**Benefits:**
- ✅ Handlers don't know about display
- ✅ Easy to test (check outcome type)
- ✅ Can change display without touching handlers
- ✅ Supports command pipelines (outcomes pass data)

### The Biggest Challenge: Continuous Input

**The problem:** Most CLI examples show single-command execution. Real CLIs need continuous input.

**The solution:** REPL (Read-Eval-Print-Loop) with proper state management.

```csharp
public abstract class CliApp
{
    public async Task Run()
    {
        OnSessionStart();
        
        while (_workflow.Status != CliWorkflowStatus.Stopped)
        {
            var run = _workflow.NextRun();
            OnRunCreated(run);
            
            var ask = Io.Ask();  // Read
            var runTask = run.RespondToAsk(ask);  // Eval
            
            OnRunStarted(run, ask);
            
            var outcomes = await runTask;
            Io.Say(outcomes);  // Print
            
            OnRunComplete(run, outcomes);
        }
        
        OnSessionEnd(_workflow.Runs);
    }
}
```

**Key features:**
- ✅ Session remains alive between commands
- ✅ Errors don't crash the session
- ✅ Lifecycle hooks for customization
- ✅ Command history maintained
- ✅ Async command execution

---

