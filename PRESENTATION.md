# From Hello World to a Scalable CLI Architecture

**"Stop writing throwaway scripts. Start building proper CLIs."**

**Duration:** 45 minutes  
**Event:** DotNetNorth  
**Speaker:** Joshua Edward Crowe  
**Repository:** KitCli/KitCli.Spendfulness

---

## Table of Contents
1. [Hook - Live Demo](#1-hook---live-demo) (2 minutes)
2. [Who Am I](#2-who-am-i) (3 minutes)
3. [What Is This Talk About](#3-what-is-this-talk-about) (5 minutes)
4. [Where Did I Cheat](#4-where-did-i-cheat) (5 minutes)
5. [LIVE DEMO - Hello World to Reusable](#5-live-demo---hello-world-to-reusable) (10 minutes)
6. [HOW IT WORKS](#6-how-it-works) (15 minutes)
7. [WHERE NEXT](#7-where-next) (3 minutes)
8. [Q&A](#8-qa) (2 minutes)

---

## 1. Hook - Live Demo
**Duration: 2 minutes**

### Let's Start With Something Simple

```bash
$ ./HelloWorldCli
> /hello
World!
```

**That's it.** A CLI that responds to `/hello` with `World!`

**But wait...** what if we want:
- Type-safe arguments?
- Multiple commands?
- Reusable across projects?
- Proper error handling?
- Command pipelines?

**That's what we're building today.** 🚀

---

## 2. Who Am I
**Duration: 3 minutes**

### Joshua Edward Crowe

**Day Job:** Software Engineer at BrightHR

### What's BrightHR?

We're **transforming people management** — making HR and employee management simple for SMEs.

**The Challenge:**
- Strong player in the market
- Growing fast
- Need to think about **architecture for the future**
- **Extensibility** is key to delivering value quickly

### Why Does This Matter?

As an engineering department scales, we need:
- **Fast tooling** for repetitive tasks
- **Proper architecture** even for "small" internal tools
- **Reusable patterns** that work across teams

**This talk is about one of those patterns: building proper CLIs.**

---

## 3. What Is This Talk About
**Duration: 5 minutes**

### The Problem: We Write Throwaway Scripts

A big part of moving forward as an engineering department is making those **small, intricate jobs** much faster.

**Common Scenario:**
- Unique task that doesn't belong in the product
- Need a quick tool to automate something
- Write a "throwaway" script
- Script becomes permanent
- Script becomes unmaintainable
- Script becomes a problem

### The Solution: Proper CLIs

We often solve these with **tooling** — writing quick little apps where:
- The task is unique enough
- Doesn't belong in the product
- Or there are bigger opportunities out there

**The CLI is a key tool in that space.**

### The Great Debate

**GitHub Desktop vs Git CLI** — the debate goes on today.

**The .NET Team** has embraced CLIs:
- `dotnet` CLI is the primary interface
- Entity Framework CLI tools
- Build and deployment tools

**Any big fan of grep** is willing to fight that corner too. 😄

### What We're Building

A **reusable CLI framework** that turns throwaway scripts into:
- **Maintainable** applications
- **Extensible** architecture
- **Type-safe** commands
- **Testable** components
- **Production-ready** tools

---

## 4. Where Did I Cheat
**Duration: 5 minutes**

### Standing on the Shoulders of Giants

There are some **already amazing packages** out there that solve key architectural challenges. Let's get those out of the way first.

#### 1. Microsoft Dependency Injection

**What it does:** Manages object lifetime and dependencies.

**Why it's great:**
- Many common frameworks already use it
- Really powerful
- Familiar to .NET developers
- Quickly spin up apps

**How we use it:**
```csharp
services.AddSingleton<ICliWorkflow, CliWorkflow>();
services.AddTransient<ICliCommandHandler, MyCommandHandler>();
```

#### 2. MediatR

**What it does:** Atomically separates business logic via CQRS pattern.

**The principle:** One handler, one action.

**Why it's great:**
- Clean separation of concerns
- Easy to test individual handlers
- Pipeline behaviors for cross-cutting concerns

**How we use it:**
```csharp
// Command
public record DeployCommand(string Environment) : IRequest<CliCommandOutcome[]>;

// Handler
public class DeployCommandHandler : IRequestHandler<DeployCommand, CliCommandOutcome[]>
{
    public async Task<CliCommandOutcome[]> Handle(DeployCommand cmd)
    {
        // Business logic here
    }
}
```

#### 3. ConsoleTables

**What it does:** A neat way to display info.

**Why it's great:**
- Most business data is relational, tabular
- Beautiful output with minimal code

**Example:**
```
┌──────────┬─────────┬───────┐
│ Service  │ Status  │ Count │
├──────────┼─────────┼───────┤
│ API      │ Running │ 3     │
│ Worker   │ Stopped │ 0     │
└──────────┴─────────┴───────┘
```

#### 4. CsvHelper

**What it does:** Go one better — export CSVs.

**Why it's great:**
- Data is often consumed in Excel/Google Sheets
- CSV is universal
- CsvHelper makes it trivial

**The Stack:**
- **Microsoft DI** for object management
- **MediatR** for command dispatch
- **ConsoleTables** for display
- **CsvHelper** for export

**What we're building:** The glue that makes these work together beautifully.

---

## 5. LIVE DEMO - Hello World to Reusable
**Duration: 10 minutes**

### Part 1: Hello World App (Start from Scratch)

**Goal:** Build the simplest possible CLI.

#### Step 1: Create the App (1 minute)
```bash
dotnet new console -n HelloWorldCli
cd HelloWorldCli
```

#### Step 2: Add Basic Loop (2 minutes)
```csharp
// Program.cs
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();
    
    if (input == "/hello")
    {
        Console.WriteLine("World!");
    }
    else if (input == "/exit")
    {
        break;
    }
}
```

**Run it:**
```bash
$ dotnet run
> /hello
World!
> /exit
```

**Problems:**
- Hardcoded commands
- No argument parsing
- Not testable
- Not reusable

### Part 2: Convert to Reusable (5 minutes)

**Goal:** Make it extensible and maintainable.

#### Step 1: Install Framework Packages
```bash
dotnet add package MediatR
dotnet add package Microsoft.Extensions.DependencyInjection
```

#### Step 2: Define Command
```csharp
// Commands/HelloCommand.cs
public record HelloCommand : CliCommand;
```

#### Step 3: Create Handler
```csharp
// Handlers/HelloCommandHandler.cs
public class HelloCommandHandler : IRequestHandler<HelloCommand, CliCommandOutcome[]>
{
    public async Task<CliCommandOutcome[]> Handle(
        HelloCommand command, 
        CancellationToken ct)
    {
        return OutcomeAs("World!");
    }
}
```

#### Step 4: Register in DI
```csharp
// Program.cs
var services = new ServiceCollection();
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
services.AddSingleton<ICliWorkflow, CliWorkflow>();

var provider = services.BuildServiceProvider();
var app = provider.GetRequiredService<CliApp>();
await app.Run();
```

**What we gained:**
- ✅ Testable (mock the handler)
- ✅ Extensible (add more commands without touching core)
- ✅ Type-safe (commands are objects, not strings)
- ✅ Reusable (framework can be extracted)

### Part 3: Filter App Example (2 minutes)

**Real-world example:** Show how reusable architecture works in the wild.

```csharp
// Filter command - completely independent
public record FilterCommand(string Field, string Value) : CliCommand;

// Handler - focused on one thing
public class FilterCommandHandler : IRequestHandler<FilterCommand, CliCommandOutcome[]>
{
    public async Task<CliCommandOutcome[]> Handle(FilterCommand cmd)
    {
        var filter = new EqualsFilter(cmd.Field, cmd.Value);
        return OutcomeAs(filter); // Can be consumed by next command!
    }
}
```

**Usage:**
```bash
> /filter --field status --value active | /table | /export-csv
```

**The power:** Commands compose. Just like Unix pipes, but type-safe.

---

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

## 7. WHERE NEXT
**Duration: 3 minutes**

### The Roadmap - What's Coming

#### 1. Side Effects Model

**Current limitation:** Commands return outcomes, but can't trigger other actions.

**Solution:** MediatR has Notifications.

```csharp
// After a command completes, publish notification
await _mediator.Publish(new CommandCompletedNotification(command));

// Multiple handlers can react
public class AuditLogger : INotificationHandler<CommandCompletedNotification>
{
    public async Task Handle(CommandCompletedNotification notification)
    {
        await _auditLog.LogCommand(notification.Command);
    }
}
```

**Use cases:**
- Audit logging
- Analytics
- Triggering related actions
- Event sourcing

#### 2. Getting Rid of MediatR (Maybe?)

**Current state:** MediatR works great, but...

**Things I want to explore:**
- Pipeline Behaviors (cross-cutting concerns)
- Validators (command validation before execution)
- Could we build this ourselves with less magic?

**The goal:** Understand if we can simplify while keeping benefits.

#### 3. Better DI Registry

**Current limitation:** Microsoft DI works, but registration is verbose.

**Inspiration:**
- Autofac's module system
- Lamar's service registries

**Goal:**
```csharp
// Instead of individual registrations
services.AddKeyedTransient<ICliCommandGenerator>("cmd1", ...);
services.AddKeyedTransient<ICliCommandGenerator>("cmd2", ...);
// ... 50 more times

// Could we do this?
services.AddCommandsFromAssembly(Assembly.GetExecutingAssembly());
```

**Find a nice balance:** Autofac/Lamar style registry while still letting Microsoft DI do most of the work.

#### 4. LLM Integration Experiment

**Wild idea:** Can we use the state model to reflect interactions with an LLM?

**Concept:**
- User input → LLM prompt
- LLM response → Command generation
- State machine tracks conversation
- Commands executed based on natural language

**Example:**
```
> I want to deploy the API to production with 5 replicas
[LLM interprets] → /deploy --env production --service api --replicas 5
[Confirmation] → Executing: deploy to production (5 replicas)
[User] → yes
✅ Deployed
```

**Challenge:** State machine for LLM conversations (context, clarification, confirmation).

### Contributing

**The framework is open source:** KitCli/KitCli.Spendfulness

**What we'd love:**
- Feedback on the architecture
- Ideas for improvement
- Real-world use cases
- PRs welcome!

---

## 8. Q&A
**Duration: 2 minutes**

### Common Questions

**Q: Why not just use System.CommandLine?**
- **A:** System.CommandLine is great for single-command CLIs. This framework is for interactive, continuous-input CLIs with command pipelines and reusable patterns.

**Q: Is this production-ready?**
- **A:** Yes! It's running in production for SpendfulnessCli (financial management tool). Comprehensive tests, ADR-documented decisions, handles edge cases.

**Q: How do I get started?**
- **A:** 
  1. Clone the repo: `git clone https://github.com/KitCli/KitCli.Spendfulness`
  2. Explore the `Cli.*` projects (the framework)
  3. Look at `SpendfulnessCli.*` for examples
  4. Build your first command!

**Q: What about testing?**
- **A:** Easy! Commands are records. Handlers are classes. Mock dependencies, test handlers. Framework provides `ICliCommandOutcomeIo` for integration tests.

**Q: Can I extend the argument type system?**
- **A:** Yes! Implement `IConsoleInstructionArgumentBuilder` for your type and register it. Framework automatically uses it.

---

## Summary

### What We Covered

1. **Hook:** Simple hello world demo
2. **Context:** Why proper CLIs matter at scale
3. **Standing on Shoulders:** MediatR, Microsoft DI, ConsoleTables, CsvHelper
4. **Live Coding:** Hello World → Reusable Framework
5. **Deep Dive:** 7 components that make it work
6. **Future:** Side effects, LLM integration, better DI

### Key Takeaways

✅ **Stop writing throwaway scripts** — build proper CLIs  
✅ **Type safety from input to handler** — no string manipulation  
✅ **Extensible architecture** — add commands without touching core  
✅ **Command pipelines** — compose simple commands into complex workflows  
✅ **Production-ready** — state machines, error handling, testing  
✅ **Reusable framework** — works for any CLI domain  

### The Big Idea

**A framework that makes building professional CLIs as easy as writing a throwaway script.**

---

## Resources

### Repository
**GitHub:** https://github.com/KitCli/KitCli.Spendfulness

### Key Documentation
- `/ADR` - Architecture Decision Records
- `CONCEPTS.md` - High-level concepts
- `Cli.*` projects - Reusable framework
- `SpendfulnessCli.*` - Example application

### Packages Used
- **MediatR** - https://github.com/jbogard/MediatR
- **Microsoft.Extensions.DependencyInjection**
- **ConsoleTables** - https://github.com/khalidabuhakmeh/ConsoleTables
- **CsvHelper** - https://joshclose.github.io/CsvHelper/

### Contact
**Twitter/X:** @joshuaedwardcrowe (probably)  
**GitHub:** @joshuaedwardcrowe  
**BrightHR:** We're hiring! 😄

---

## Thank You! 🎉

**Questions? Let's discuss!**

*"The difference between a script and a tool is whether you're proud to show it to someone else."*

**Remember:** Stop writing throwaway scripts. Start building proper CLIs.

**Good luck building! 🚀**
