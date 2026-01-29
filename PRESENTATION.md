# Building Extensible CLI Applications: A Framework Approach 🚀

**Duration:** 45 minutes  
**Presenter:** [Your Name]  
**Repository:** KitCli/SpendfulnessCli

---

## Table of Contents
1. [What Is This Framework?](#1-what-is-this-framework) (8 minutes)
2. [Framework Features Demo](#2-framework-features-demo) (10 minutes)
3. [The Architecture That Makes It Work](#3-the-architecture-that-makes-it-work) (12 minutes)
4. [Why It Doesn't Suck: SOLID, DRY, YAGNI](#4-why-it-doesnt-suck-solid-dry-yagni) (10 minutes)
5. [Live Coding: Build Your CLI](#5-live-coding-build-your-cli) (3 minutes)
6. [Q&A](#6-qa) (2 minutes)

---

## 1. What Is This Framework?
**Duration: 8 minutes**

### Why CLIs Still Matter in 2026 🖥️

**Let's be honest:** CLIs might seem old-school in a world of fancy UIs and web apps.

**But here's the thing:** CLIs are **developer superpowers** for:

**⚡ Speed & Automation**
- Type commands vs clicking through 5 screens
- Chain commands together: no manual data copying
- Script repetitive tasks: run weekly reports automatically

**🔧 Developer Tooling**
- Git, npm, docker — the best tools are CLIs
- Integrates into your existing workflow
- Perfect for CI/CD pipelines and automation

**🎯 Custom Analysis**
- GUI apps are limited to what someone built
- CLIs let YOU decide what questions to ask
- Build exactly the tool you need

**💪 Power User Paradise**
- Keyboard > mouse for power users
- Composable: combine simple commands into complex workflows
- Scriptable: automate everything

**Real Talk:** If you're a developer, you probably spend half your day in a terminal anyway. Why not make your tools work the way YOU work?

### TL;DR

This is a **reusable CLI framework** (the `Cli.*` projects) that gives you:
- 🏗️ Production-ready CLI infrastructure
- 🔌 Plugin architecture - add commands in minutes
- 🔍 Type-safe command parsing (no more string manipulation!)
- 🔄 Command pipelines (compose simple commands into complex workflows)
- 📦 Reusable patterns for any CLI application
- ✅ Proven architecture used in real production app (SpendfulnessCli)

### The Framework vs The Application

**The Framework (`Cli.*` projects):** Reusable infrastructure for building ANY CLI
- `Cli` - Core application loop and lifecycle
- `Cli.Abstractions` - Base abstractions (I/O, tables, aggregators)
- `Cli.Commands.Abstractions` - Command pattern infrastructure
- `Cli.Instructions` - Instruction parsing pipeline
- `Cli.Workflow` - Workflow and state management

**The Application (`SpendfulnessCli.*`):** One example built on the framework
- Domain-specific commands (financial analysis)
- Custom aggregations
- YNAB integration
- Proves the framework works in production!

**You can build:** Developer tools, data processors, automation scripts, monitoring CLIs, deployment tools, testing frameworks, etc.

---

## 2. Framework Features Demo
**Duration: 10 minutes**

### Feature 1: Type-Safe Command Parsing 🔍

**The Problem:** Most CLIs do ugly string parsing everywhere.

**Before (typical CLI code):**
```csharp
// Ugh, brittle string manipulation
var parts = input.Split(' ');
var command = parts[0];
var arg1 = parts[1]; // Hope this exists!
var value = int.Parse(parts[2]); // Hope this is a number!
```

**With This Framework:**
```csharp
// Type-safe from input to handler!
var instruction = parser.Parse("/deploy --environment production --replicas 3");
// instruction.Name = "deploy"
// instruction.Arguments = [
//   TypedArgument<string>("environment", "production"),
//   TypedArgument<int>("replicas", 3)
// ]
```

**Why It's Cool:**
- ✅ **Three-stage pipeline:** Find tokens → Extract values → Convert to types
- ✅ **Plugin-based type system:** Add support for new types (Guid, DateOnly, custom types)
- ✅ **Compiler-checked:** No runtime type errors
- ✅ **IntelliSense works:** Refactoring is safe

**Real Example from Production:**
```bash
/filter-transactions --payee-name "Amazon" --amount-greater-than 50
# Framework automatically converts:
# "Amazon" → string
# 50 → decimal
# Your handler receives typed arguments!
```

### Feature 2: Plugin Architecture (Add Commands in 2 Minutes!) ⚡

**The Problem:** Adding features to CLIs is usually painful.

**With This Framework:**

**Step 1:** Define command (30 seconds)
```csharp
public record DeployCommand(string Environment, int Replicas) : CliCommand;
```

**Step 2:** Write handler (60 seconds)
```csharp
public class DeployCommandHandler 
    : IRequestHandler<DeployCommand, CliCommandOutcome[]>
{
    public async Task<CliCommandOutcome[]> Handle(DeployCommand cmd)
    {
        // Your logic here
        await DeployService.Deploy(cmd.Environment, cmd.Replicas);
        return OutcomeAs($"Deployed to {cmd.Environment} with {cmd.Replicas} replicas");
    }
}
```

**Step 3:** Register (30 seconds)
```csharp
services.AddKeyedTransient<ICliCommandGenerator>(
    "deploy",
    (sp, key) => new DeployCommandGenerator()
);
```

**That's it!** Framework handles:
- ✅ Parsing `/deploy --environment production --replicas 3`
- ✅ Converting arguments to correct types
- ✅ Routing to your handler
- ✅ Error handling
- ✅ Displaying output

**No changes to core code. Ever.**

### Feature 3: Command Pipelines (Unix Pipes, But Type-Safe!) 🔗

**The Problem:** Most CLIs make you run commands separately and manually copy data.

**Framework Solution:**
```bash
# Unix pipes (strings only)
cat file.txt | grep "error" | wc -l

# This framework (typed data!)
/load-logs | /filter --level error | /count
```

**How It Works:**
- Commands return **typed outcomes** (not just strings!)
- Next command receives **typed data** as properties
- Type-safe composition with compiler verification
- Build complex workflows from simple commands

**Example Flow:**
```
/load-logs → LogsOutcome (List<LogEntry>)
       ↓
/filter --level error → FilteredLogsOutcome (List<LogEntry>)
       ↓
/count → MessageOutcome ("Found 42 errors")
```

**Why This Is Powerful:**
- ✅ No writing one-off "mega commands"
- ✅ Users compose features themselves
- ✅ Type safety prevents errors
- ✅ Infinite combinations from finite commands

### Feature 4: Reusable Aggregation Pattern 📦

**The Problem:** Same data manipulation logic everywhere.

**Framework Solution: Aggregators**

```csharp
// Define once
public class MonthlyTotalAggregator : CliListAggregator<MonthTotal>
{
    protected override List<MonthTotal> OnAggregate()
    {
        return Data
            .GroupBy(item => new { item.Year, item.Month })
            .Select(g => new MonthTotal(g.Key.Year, g.Key.Month, g.Sum(x => x.Amount)))
            .ToList();
    }
}

// Use everywhere with composition
var aggregator = new MonthlyTotalAggregator(data)
    .BeforeAggregation(a => a.FilterByDateRange(start, end))
    .BeforeAggregation(a => a.FilterByCategory("groceries"))
    .AfterAggregation(a => a.OrderByMonth());

var results = aggregator.Aggregate();
```

**Benefits:**
- ✅ Write logic once, use in multiple commands
- ✅ Test once
- ✅ Fix bugs once
- ✅ Fluent composition (chain operations)

### Feature 5: Built-in CLI Table Formatting 📊

**The Problem:** Making pretty tables in CLIs is tedious.

**Framework Solution:**
```csharp
var table = new CliTable();
table.AddColumn("Name");
table.AddColumn("Status");
table.AddColumn("Count");

table.AddRow("Service-A", "Running", "3");
table.AddRow("Service-B", "Stopped", "0");

return OutcomeAs(table);
```

**Output:**
```
┌───────────┬─────────┬───────┐
│ Name      │ Status  │ Count │
├───────────┼─────────┼───────┤
│ Service-A │ Running │ 3     │
│ Service-B │ Stopped │ 0     │
└───────────┴─────────┴───────┘
```

**Features:**
- ✅ Automatic column sizing
- ✅ Sorting support
- ✅ Pagination
- ✅ Unicode box drawing
- ✅ Consistent formatting

### Feature 6: Interactive Session Management 🔄

**The Framework Handles:**
- ✅ **REPL Loop:** Read-Eval-Print-Loop for interactive sessions
- ✅ **Session State:** Track command history
- ✅ **Lifecycle Hooks:** `OnSessionStart`, `OnRunCreated`, `OnRunComplete`, `OnSessionEnd`
- ✅ **Error Recovery:** Graceful error handling, session continues
- ✅ **State Transitions:** Validated state machine (prevents bugs)

**Example:**
```csharp
public class MyCliApp : CliApp
{
    protected override void OnSessionStart()
    {
        Io.Say("Welcome to My CLI!");
    }
    
    protected override void OnRunComplete(ICliWorkflowRun run, CliCommandOutcome[] outcomes)
    {
        // Log every command executed
        Logger.LogCommand(run.Instruction);
    }
}
```

**You Focus On:** Your command logic  
**Framework Handles:** Everything else

---

## 3. The Architecture That Makes It Work
**Duration: 12 minutes**

### How Do You Build a Framework That Doesn't Suck?

**The Challenge:** Support unlimited commands without becoming spaghetti code.

**The Solution:** Smart architecture patterns that make it EASY to extend.

### Pattern 1: The Three-Layer Separation 🎂

```
┌─────────────────────────────────────┐
│     You Type: "/deploy"             │  ← User Layer (CliApp)
└─────────────┬───────────────────────┘
              │
     ┌────────▼─────────┐
     │  Parse & Route   │  ← Workflow Layer (CliWorkflow)
     └────────┬─────────┘
              │
     ┌────────▼─────────┐
     │  Execute Logic   │  ← Command Layer (Handlers)
     └──────────────────┘
```

**Layer 1: CliApp (User Interaction)**
```csharp
public abstract class CliApp
{
    public async Task Run()
    {
        OnSessionStart();
        while (_workflow.Status != CliWorkflowStatus.Stopped)
        {
            var ask = Io.Ask();              // Get input
            var outcomes = await run.RespondToAsk(ask);
            Io.Say(outcomes);                // Display output
        }
    }
}
```
- Shows prompts, gets input, displays results
- **No business logic here**
- Override lifecycle hooks for customization

**Layer 2: CliWorkflow (Traffic Cop)**
```csharp
public class CliWorkflowRun
{
    public async Task<CliCommandOutcome[]> RespondToAsk(string? ask)
    {
        var instruction = _parser.Parse(ask);        // Parse
        var command = _provider.GetCommand(instruction); // Route
        return await _mediator.Send(command);         // Execute
    }
}
```
- Parses `/deploy --environment prod` into typed command
- Routes to the right handler
- Manages session state

**Layer 3: Command Handlers (The Actual Work)**
```csharp
public class DeployCommandHandler : IRequestHandler<DeployCommand, CliCommandOutcome[]>
{
    public async Task<CliCommandOutcome[]> Handle(DeployCommand cmd)
    {
        // Your logic here
        await _deployService.Deploy(cmd.Environment);
        return OutcomeAs($"Deployed to {cmd.Environment}!");
    }
}
```
- Implements your business logic
- Independent, testable, focused

**Why This Matters:**
- ✅ Each layer has ONE responsibility (SRP)
- ✅ Easy to test each piece in isolation
- ✅ Change one layer without breaking others
- ✅ New developers understand quickly

### Pattern 2: Parser Three-Stage Pipeline 🔍

**Problem:** Converting `"/deploy --env prod"` into typed objects.

**Solution:** Three specialized stages:

**Stage 1: Token Indexer**
```
Input:  "/deploy --environment production"
Output: PrefixIndex=0, NameIndex=1-6, ArgIndex=8...
```
- **One job:** Find where tokens are located
- Fast string scanning
- No parsing logic

**Stage 2: Token Extractor**
```
Output: {
    Prefix: "/",
    Name: "deploy",
    Arguments: { "environment": "production" }
}
```
- **One job:** Extract string values
- Uses indexes from Stage 1
- Dictionary of arg name → value

**Stage 3: Argument Builders**
```csharp
public interface IConsoleInstructionArgumentBuilder
{
    bool For(string? value);  // Can I handle this?
    ConsoleInstructionArgument Create(string name, string? value);
}
```
- **One job per builder:** Convert one type
- `IntBuilder`, `DateBuilder`, `GuidBuilder`, `BoolBuilder` (fallback)
- Plugin system: register new builders for custom types

**Result:**
```csharp
public record ConsoleInstruction(
    string Name,
    IEnumerable<TypedConsoleInstructionArgument> Arguments
);
```

**Why Three Stages?**
- ✅ Single Responsibility per stage
- ✅ Easy to test each stage
- ✅ Easy to extend (add new type builders)
- ✅ Clear error messages per stage

### Pattern 3: MediatR for Command Dispatch 📬

**Instead of:** Big switch statement or if/else chains

**Use:** MediatR pattern (CQRS)

```csharp
// Command = Data (no logic)
public record DeployCommand(string Environment) : CliCommand;

// Handler = Logic (no routing)
public class DeployCommandHandler 
    : IRequestHandler<DeployCommand, CliCommandOutcome[]>
{
    public async Task<CliCommandOutcome[]> Handle(DeployCommand cmd)
    {
        // Business logic here
    }
}

// Routing = Automatic
var outcome = await _mediator.Send(command);
```

**Benefits:**
- ✅ Commands are simple data carriers
- ✅ Handlers are focused on business logic
- ✅ No routing code in your app
- ✅ Can add pipeline behaviors (logging, validation, caching)
- ✅ Handlers can be in different assemblies

### Pattern 4: Outcome Pattern (No Exceptions for Control Flow) ✅

**Problem:** Using exceptions for normal control flow is expensive and unclear.

**Solution:** Typed outcomes

```csharp
// Base outcome
public abstract class CliCommandOutcome { }

// Specific outcomes
public class CliCommandTableOutcome(CliTable table) : CliCommandOutcome;
public class CliCommandOutputOutcome(string message) : CliCommandOutcome;
public class CliCommandNothingOutcome : CliCommandOutcome;
public class CliCommandExceptionOutcome(Exception ex) : CliCommandOutcome;
public class FilterCliCommandOutcome(CliListAggregatorFilter filter) : CliCommandOutcome;
```

**Usage:**
```csharp
// Return success
return OutcomeAs("Deployment successful!");

// Return table
return OutcomeAs(table);

// Return filter (for pipelines!)
return OutcomeAs(filter);
```

**Why This Pattern:**
- ✅ Explicit success/failure handling
- ✅ Type-safe result passing
- ✅ Enables command pipelines
- ✅ I/O layer decides how to display
- ✅ No hidden control flow

### Pattern 5: Dependency Injection Throughout 💉

**Framework Philosophy:** Everything is injected, nothing is `new`'d

```csharp
// Register framework
services.AddSingleton<ICliInstructionParser, ConsoleInstructionParser>();
services.AddSingleton<ICliWorkflow, CliWorkflow>();
services.AddSingleton<ICliCommandOutcomeIo, ConsoleCliCommandOutcomeIo>();

// Register commands
services.AddKeyedTransient<ICliCommandGenerator>("deploy", ...);

// Register type builders
services.AddSingleton<IConsoleInstructionArgumentBuilder, IntBuilder>();
services.AddSingleton<IConsoleInstructionArgumentBuilder, DateBuilder>();
```

**Benefits:**
- ✅ Easy to test (inject mocks)
- ✅ Swap implementations (ConsoleIo → FileIo → TestIo)
- ✅ Plugin architecture (register new commands/types)
- ✅ Open/Closed Principle in action

---

## 4. Why It Doesn't Suck: SOLID, DRY, YAGNI

**Most CLIs:**
```csharp
// Ugh, string parsing everywhere
var parts = input.Split(' ');
var command = parts[0];
var arg1 = parts[1]; // Hope this exists!
var value = int.Parse(parts[2]); // Hope this is a number!
```

**SpendfulnessCli:**
```csharp
// Type-safe from the start!
var instruction = parser.Parse("/spare-money --minus-savings true");
// instruction.Name = "spare-money"
// instruction.Arguments = [TypedArgument<bool>("minus-savings", true)]

// Later, in your handler:
public class SpareMoneyHandler(bool? MinusSavings) // Compiler-checked!
```

**The Magic:** Three-stage pipeline
1. **Find** where tokens are → `"--minus-savings true"` starts at position 13
2. **Extract** the values → `{ "minus-savings": "true" }`
3. **Convert** to types → `TypedArgument<bool>(true)`

**Why This Rocks:**
- ✅ No string parsing in business logic
- ✅ Compiler catches type errors
- ✅ Refactoring is safe
- ✅ IntelliSense just works

### Pattern 3: Plugin Architecture (Add Commands in 2 Minutes!) ⚡

**Want to add a new command?** Just three steps:

```csharp
// 1. Define the command (data only)
public record MyAwesomeCommand(string Param) : CliCommand;

// 2. Write the handler (the logic)
public class MyAwesomeCommandHandler 
    : IRequestHandler<MyAwesomeCommand, CliCommandOutcome[]>
{
    public async Task<CliCommandOutcome[]> Handle(MyAwesomeCommand cmd)
    {
        // Do your thing
        return OutcomeAs("Result!");
    }
}

// 3. Register it
services.AddKeyedTransient<ICliCommandGenerator>(
    "my-awesome-command",
    (sp, key) => new MyAwesomeCommandGenerator()
);
```

**That's it!** The framework:
- ✅ Automatically finds your command
- ✅ Parses arguments for you
- ✅ Routes `/my-awesome-command` to your handler
- ✅ Handles errors
- ✅ Displays output

**No changes to core code. Ever.**

### Pattern 4: Reusable Data Aggregations 📦

**Problem:** Same data manipulation logic everywhere.

```csharp
// ❌ DON'T: Copy-paste filtering logic
public void Command1()
{
    var filtered = transactions
        .Where(t => t.Date > startDate)
        .Where(t => t.Amount > 0)
        .GroupBy(t => t.Category);
}

public void Command2()
{
    var filtered = transactions  // DUPLICATE!
        .Where(t => t.Date > startDate)
        .Where(t => t.Amount > 0)
        .GroupBy(t => t.Category);
}
```

**Better:** Reusable aggregators

```csharp
// ✅ DO: Write once, use everywhere
public class TransactionMonthTotalAggregator
{
    protected override List<MonthTotal> OnAggregate()
    {
        return Transactions
            .GroupBy(t => new { t.Date.Year, t.Date.Month })
            .Select(g => new MonthTotal(g.Key.Year, g.Key.Month, g.Sum(t => t.Amount)))
            .ToList();
    }
}

// Use in multiple commands with composition
var aggregator = new TransactionMonthTotalAggregator(transactions)
    .BeforeAggregation(a => a.FilterToDateRange(start, end))
    .AfterAggregation(a => a.OrderByYear());

var results = aggregator.Aggregate();
```

**Benefits:**
- ✅ Write logic once
- ✅ Test once
- ✅ Fix bugs once
- ✅ Compose operations fluently

### Pattern 5: Command Pipelines (Unix Pipes, But Better!) 🔗

**Unix Pipes:**
```bash
cat file.txt | grep "error" | sort | uniq
```

**SpendfulnessCli Pipes:**
```bash
/filter-transactions --payee "Amazon" | /table | /export-csv
```

**How It Works:**
- Commands return typed outcomes (not just strings!)
- Next command receives typed data
- Type-safe composition
- Infinite possibilities

**Example Flow:**
```
/filter-transactions → TransactionOutcome[]
       ↓
/table → TableOutcome
       ↓
/export-csv → FileOutcome
```

**Why This Is Powerful:**
- ✅ Build complex reports from simple commands
- ✅ No need to write custom one-off commands
- ✅ Users compose features themselves

---

## 4. Why It Doesn't Suck: SOLID, DRY, YAGNI
**Duration: 10 minutes**

### The Real Question: How Do You Keep This Maintainable?

**Bad Code Smells to Avoid:**
- 🤮 Copy-paste programming
- 🤮 God classes that do everything
- 🤮 Deep inheritance hierarchies
- 🤮 Building features "just in case"

### SOLID: The Cheat Codes for Good Code

#### S - Single Responsibility Principle
**Translation:** Each class does ONE thing.

```csharp
// ❌ BAD: Class does too much
public class SpareMoneyHandler
{
    public void Handle()
    {
        // Parse input
        // Query database
        // Calculate money
        // Format output
        // Display to user
    }
}

// ✅ GOOD: Separate concerns
public class CliApp { /* Only handles user I/O */ }
public class CliWorkflow { /* Only routes commands */ }
public class SpareMoneyHandler { /* Only calculates spare money */ }
```

**Why It Matters:** When spare money calculation changes, you only touch ONE class.

#### O - Open/Closed Principle
**Translation:** Add features without changing existing code.

```csharp
// Want a new command? Just register it!
services.AddKeyedTransient<ICliCommandGenerator>(
    "my-new-command",
    (sp, key) => new MyNewCommandGenerator()
);
// No changes to core framework needed!
```

**Why It Matters:** Add 50 new commands without touching the command router. Ship faster!

#### L - Liskov Substitution Principle
**Translation:** Subtypes should work wherever parent types work.

```csharp
// Any ICliCommandOutcome works the same way
CliCommandOutcome outcome = new TableOutcome(...);
CliCommandOutcome outcome = new MessageOutcome(...);
CliCommandOutcome outcome = new FilterOutcome(...);
// All handled uniformly by the framework
```

**Why It Matters:** Consistent behavior = fewer bugs.

#### I - Interface Segregation Principle
**Translation:** Small, focused interfaces > big bloated ones.

```csharp
// ✅ GOOD: Focused interfaces
public interface ICliCommandGenerator
{
    CliCommand Generate(CliInstruction instruction);
}

public interface ICliCommandPropertyFactory
{
    bool CanCreateProperty(CliCommandOutcome outcome);
    CliCommandProperty CreateProperty(CliCommandOutcome outcome);
}

// ❌ BAD: One mega-interface
public interface ICliCommandEverything
{
    CliCommand Generate(...);
    bool CanCreateProperty(...);
    void Validate(...);
    void Log(...);
    void Export(...);
    // ... 20 more methods
}
```

**Why It Matters:** Implement only what you need. Simpler code, faster development.

#### D - Dependency Inversion Principle
**Translation:** Depend on abstractions, not concrete classes.

```csharp
// ✅ GOOD: Depend on interface
public class CliApp(ICliCommandOutcomeIo io)
{
    // Can inject different implementations:
    // - ConsoleIo for production
    // - TestIo for testing
    // - FileIo for scripting
}

// ❌ BAD: Depend on concrete class
public class CliApp
{
    private readonly ConsoleIo io = new ConsoleIo();
    // Now stuck with Console forever!
}
```

**Why It Matters:** Easy testing, flexibility, swappable components.

### DRY: Don't Repeat Yourself

**The Sin:** Copy-paste code everywhere.

**The Fix:** Reusable components.

```csharp
// ✅ Write once, use everywhere
public class TransactionMonthTotalAggregator { /* ... */ }

// Use in command 1
var aggregator1 = new TransactionMonthTotalAggregator(data).Aggregate();

// Use in command 2
var aggregator2 = new TransactionMonthTotalAggregator(otherData).Aggregate();
```

**Real Examples in SpendfulnessCli:**
- **Aggregators:** Reusable data transformations
- **Base Handlers:** Common outcome creation methods
- **Extension Methods:** Shared string utilities
- **Constants:** Single source of truth for parsing rules

**Result:** Fix a bug once, it's fixed everywhere.

### YAGNI: You Aren't Gonna Need It

**The Trap:** "We might need this someday!"

**The Reality:** You won't. And if you do, add it then.

```csharp
// ✅ GOOD: Simple state machine (just what's needed)
public enum CliWorkflowStatus
{
    Started,
    Stopped
}

// ❌ BAD: Over-engineered "just in case"
public enum CliWorkflowStatus
{
    Started,
    Paused,           // Not needed yet
    Suspended,        // Not needed yet
    Hibernating,      // Definitely not needed
    QuantumSuperposition,  // What even is this?
    Stopped
}
```

**Real Examples:**
- **I/O Interface:** Just `Ask()` and `Say()` — that's all we need!
- **Session States:** Just `Started` and `Stopped` — simple!
- **No Premature Optimization:** Use stopwatch for timing, not distributed tracing

**The Philosophy:** Build what you need today. Future you will thank you for the simplicity.

---

## 5. Live Coding: Build Your CLI
**Duration: 3 minutes**

**Let's build a deployment CLI command in real-time!**

### Step 1: Define the Command (30 seconds)
```csharp
public record DeployCommand(string Environment, int Replicas) : CliCommand;
```

### Step 2: Write the Handler (60 seconds)
```csharp
public class DeployCommandHandler 
    : IRequestHandler<DeployCommand, CliCommandOutcome[]>
{
    private readonly IDeploymentService _deployService;
    
    public async Task<CliCommandOutcome[]> Handle(
        DeployCommand command, 
        CancellationToken ct)
    {
        await _deployService.Deploy(command.Environment, command.Replicas);
        var message = $"✅ Deployed to {command.Environment} with {command.Replicas} replicas";
        return OutcomeAs(message);
    }
}
```

### Step 3: Create the Generator (30 seconds)
```csharp
public class DeployCommandGenerator : ICliCommandGenerator
{
    public CliCommand Generate(CliInstruction instruction)
    {
        var env = instruction.GetArgument<string>("environment");
        var replicas = instruction.GetArgument<int>("replicas");
        return new DeployCommand(env, replicas);
    }
}
```

### Step 4: Register It (30 seconds)
```csharp
services.AddKeyedTransient<ICliCommandGenerator>(
    "deploy",
    (sp, key) => new DeployCommandGenerator()
);
```

### Step 5: Run It! (30 seconds)
```bash
$ /deploy --environment production --replicas 5
> ✅ Deployed to production with 5 replicas
```

**That's it!** Command added in 2.5 minutes. The framework handles:
- ✅ Parsing `/deploy --environment production --replicas 5`
- ✅ Converting `"production"` to string and `5` to int
- ✅ Routing to your handler
- ✅ Injecting dependencies (`IDeploymentService`)
- ✅ Displaying the output
- ✅ Error handling

**Build ANY CLI:** Deployment tools, log analyzers, data processors, test runners, automation scripts...

---

## 6. Q&A
**Duration: 2 minutes**

### Common Questions

**Q: Can I use this framework for my own CLI app?**
- **A:** Absolutely! The core `Cli.*` projects are completely reusable. Just reference them and build your domain-specific commands. The framework is generic - it doesn't know or care about finance, deployment, or any specific domain.

**Q: Is this production-ready?**
- **A:** Yes! It's being used in production for SpendfulnessCli (financial management). The framework has comprehensive tests, ADRs documenting decisions, and handles edge cases.

**Q: How hard is it to add a command?**
- **A:** You just saw it — about 2-3 minutes. The framework handles parsing, routing, error handling, and display. You just write the business logic.

**Q: What about testing?**
- **A:** Easy! Commands are just records (data). Handlers are just classes with dependencies. Mock the dependencies, test the handler. The framework provides `ICliCommandOutcomeIo` abstraction for integration tests.

**Q: Can I add custom argument types?**
- **A:** Yes! Implement `IConsoleInstructionArgumentBuilder` for your type and register it. The framework will automatically use it during parsing.

**Q: What if I need async commands?**
- **A:** Already supported! Handlers return `Task<CliCommandOutcome[]>`. The framework handles the async execution.

**Q: Can commands call other commands?**
- **A:** Yes! Through command pipelines. Commands return outcomes that can be consumed by other commands. Type-safe composition.

**Q: What's included in the framework?**
- **A:** 
  - Core CLI loop and lifecycle management
  - Type-safe instruction parsing (3-stage pipeline)
  - Command registration and routing (via MediatR)
  - Workflow and state management
  - Command pipeline support
  - Table formatting
  - Aggregation patterns
  - All abstracted and testable!

---

## Summary: The Big Ideas

### What Makes This Framework Awesome?

1. **Reusable Infrastructure** 🏗️
   - Production-ready CLI application loop
   - Type-safe command parsing (no string manipulation!)
   - Plugin architecture (add commands in 2 minutes)
   - Command pipelines (compose simple → complex)
   - Built-in table formatting
   - Aggregation patterns

2. **Smart Architecture** 🎯
   - Three-layer separation (User, Workflow, Commands)
   - MediatR for command dispatch (CQRS pattern)
   - Three-stage parser pipeline (Index → Extract → Build)
   - Outcome pattern (no exceptions for control flow)
   - Dependency injection throughout

3. **Developer Experience** 💎
   - Easy to extend (2-minute commands)
   - Easy to test (all abstractions mockable)
   - Easy to understand (clear layer separation)
   - Well-documented (13 ADRs explaining decisions)
   - Proven in production (SpendfulnessCli uses it)

### Key Takeaways

- ✅ **Build ANY CLI with this framework** — not just financial tools
- ✅ **SOLID, DRY, YAGNI are practical** — they enable the 2-minute commands
- ✅ **Type safety prevents bugs** — compiler catches errors, not users
- ✅ **Composition over configuration** — pipelines, aggregators, outcomes
- ✅ **Framework vs Application** — `Cli.*` is reusable, `SpendfulnessCli.*` is one example

### Want to Explore More?

**Check out the repo:**
- `/ADR` - Architecture Decision Records explaining design choices
- `CONCEPTS.md` - High-level patterns and concepts
- `Cli.*` projects - The reusable framework
- `SpendfulnessCli.*` projects - Example application built on the framework
- Test projects - Examples of testing approach

**Build Your Own CLI:**
1. Reference the `Cli.*` projects
2. Create your domain-specific commands
3. Register them in DI
4. Run!

**Examples You Could Build:**
- Deployment automation tools
- Log analysis CLIs
- Data processing pipelines
- Test framework runners
- Monitoring and alerting tools
- CI/CD helpers
- Database migration tools
- API testing tools

---

## Thank You! 🎉

**Questions? Let's discuss!**

*"Great frameworks are invisible—you only notice them when they're missing."*

**Repository:** https://github.com/KitCli/SpendfulnessCli
**Fun fact:** The framework (`Cli.*`) is completely domain-agnostic. SpendfulnessCli is just one application built on it!

---

## Resources

### In the Repository
- **`Cli.*` Projects** - The reusable framework (this is what you want!)
- **`SpendfulnessCli.*` Projects** - Example application using the framework
- **ADRs** - Architecture decisions that shaped the framework
- **CONCEPTS.md** - High-level overview of patterns
- **Tests** - Comprehensive test coverage

### External Learning
- **Clean Architecture** by Robert C. Martin
- **Domain-Driven Design** by Eric Evans
- **Refactoring** by Martin Fowler
- **MediatR** - https://github.com/jbogard/MediatR
- **CQRS Pattern** - Command Query Responsibility Segregation

### Get Started!
1. Clone the repository
2. Explore the `Cli.*` projects (the framework)
3. Look at `SpendfulnessCli.*` for examples
4. Build your own CLI command
5. Reference the framework in your own projects!

**Good luck building maintainable CLIs! 🚀**
