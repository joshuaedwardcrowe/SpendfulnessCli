# SpendfulnessCli: Building a Fun Financial CLI with Great Architecture 🚀

**Duration:** 45 minutes  
**Presenter:** [Your Name]  
**Repository:** KitCli/SpendfulnessCli

---

## Table of Contents
1. [What Does This Thing Actually Do?](#1-what-does-this-thing-actually-do) (8 minutes)
2. [Cool Features Demo](#2-cool-features-demo) (10 minutes)
3. [The Secret Sauce: Smart Architecture](#3-the-secret-sauce-smart-architecture) (12 minutes)
4. [Why It Doesn't Suck: SOLID, DRY, YAGNI](#4-why-it-doesnt-suck-solid-dry-yagni) (10 minutes)
5. [Live Coding: Add Your Own Command](#5-live-coding-add-your-own-command) (3 minutes)
6. [Q&A](#6-qa) (2 minutes)

---

## 1. What Does This Thing Actually Do?
**Duration: 8 minutes**

### TL;DR

SpendfulnessCli is a **terminal-based financial management tool** that makes YNAB (You Need A Budget) data actually useful through:
- 💰 Smart financial analysis
- 🤖 AI-powered insights with ChatGPT
- 📊 Beautiful CLI tables
- 🔄 Reusable data aggregations
- 🎯 Composable command pipelines

### Not Your Average CRUD App

```bash
# Find out how much money you actually have to spend
/spare-money --minus-savings true

# See your average yearly spending with inflation tracking
/average-yearly-spending

# Ask ChatGPT about your finances (yes, really!)
/chat --prompt "What categories should I cut back on?"

# Export personal inflation rate to CSV
/personal-inflation-rate export-csv

# Find all recurring transactions automatically
/recurring-transactions
```

### Why Build This?

**The Problem:** YNAB is great, but sometimes you need custom analysis, automation, and insights that don't exist in the app.

**The Solution:** Build an extensible CLI that:
- ✅ Talks to YNAB API
- ✅ Stores custom data locally (SQLite)
- ✅ Composes data in powerful ways
- ✅ Integrates with AI for insights
- ✅ Exports to formats you can use

**The Bonus:** Learn to build actually maintainable software while solving a real problem!

---

## 2. Cool Features Demo
**Duration: 10 minutes**

### Feature 1: Spare Money Calculator 💰

**The Problem:** "How much money can I actually spend right now?"

```bash
/spare-money
# → Shows available funds across all accounts

/spare-money --minus-savings true
# → Excludes savings accounts (money you shouldn't touch!)

/spare-money --add 500 --minus 200
# → Hypothetical calculations: "What if I get paid $500 but owe $200?"
```

**Why It's Cool:**
- Real-time calculation across multiple accounts
- Flexible filtering (exclude savings, credit cards, etc.)
- Instant "what-if" scenarios

### Feature 2: AI-Powered Financial Insights 🤖

**The Problem:** Staring at numbers doesn't give you insights.

```bash
/chat --prompt "What categories am I overspending in?"
/chat --prompt "Suggest where I can cut $200 per month"
/chat --prompt "Am I spending more on dining out this year?"
```

**How It Works:**
1. Preload your transaction data into a vector database
2. ChatGPT analyzes your spending patterns
3. Get natural language insights about YOUR money

**Why It's Cool:**
- Turns data into actionable advice
- Ask questions in plain English
- Uses your actual transaction history

### Feature 3: Recurring Transaction Detection 🔄

**The Problem:** Which charges are subscriptions? Which are one-offs?

```bash
/recurring-transactions
```

**Output:**
```
┌────────────────────┬───────────┬──────────────┐
│ Payee              │ Amount    │ Frequency    │
├────────────────────┼───────────┼──────────────┤
│ Netflix            │ $15.99    │ Monthly      │
│ Gym Membership     │ $45.00    │ Monthly      │
│ Annual Insurance   │ $1,200    │ Yearly       │
└────────────────────┴───────────┴──────────────┘
```

**Why It's Cool:**
- Automatically detects patterns in your transactions
- No manual tagging required
- Spot subscriptions you forgot about!

### Feature 4: Personal Inflation Rate 📈

**The Problem:** How is inflation affecting YOUR spending?

```bash
/personal-inflation-rate export-csv
```

**What It Calculates:**
- Your personal inflation rate based on actual spending
- Category-by-category price increases
- Compare to CPI (national inflation)

**Why It's Cool:**
- National inflation numbers don't reflect YOUR situation
- See which categories hit you hardest
- Export to spreadsheet for analysis

### Feature 5: Composable Command Pipelines 🔧

**The Problem:** Most CLIs make you run commands separately.

```bash
# Filter transactions, THEN display as table, THEN export
/filter-transactions --payee-name "Amazon" | 
/table | 
/export-csv
```

**How It Works:**
- Commands pass data to the next command
- Like Unix pipes, but type-safe!
- Build complex workflows from simple commands

**Why It's Cool:**
- No need to write custom reports
- Compose features you already have
- Infinite possibilities from finite commands

### Feature 6: Average Yearly Spending Trends 📊

```bash
/average-yearly-spending

┌──────┬─────────────┬──────────────┐
│ Year │ Average     │ % Change     │
├──────┼─────────────┼──────────────┤
│ 2022 │ $4,250/mo   │ -            │
│ 2023 │ $4,680/mo   │ +10.1%       │
│ 2024 │ $5,120/mo   │ +9.4%        │
└──────┴─────────────┴──────────────┘
```

**Why It's Cool:**
- Tracks spending trends over time
- Adjusts for inflation automatically
- Visualizes year-over-year changes

---

## 3. The Secret Sauce: Smart Architecture

**Duration: 12 minutes**

### How Do All These Features Work Without Becoming Spaghetti Code?

**The Challenge:** Add 50+ commands without creating a mess.

**The Solution:** Smart architecture patterns that make it EASY to add features.

### Pattern 1: The Three-Layer Cake 🎂

```
┌─────────────────────────────────────┐
│     You Type: "/spare-money"        │  ← User Layer
└─────────────┬───────────────────────┘
              │
     ┌────────▼─────────┐
     │  Parse & Route   │  ← Workflow Layer
     └────────┬─────────┘
              │
     ┌────────▼─────────┐
     │  Execute Logic   │  ← Command Layer
     └──────────────────┘
```

**Layer 1: User Interaction**
- Shows prompts
- Gets your input
- Displays results
- That's it! No business logic here.

**Layer 2: Traffic Cop**
- Parses `/spare-money` into a command object
- Routes to the right handler
- Manages the session (start/stop)

**Layer 3: The Actual Work**
- Calculates spare money
- Queries the database
- Formats output
- Returns results

**Why This Matters:**
- ✅ Each layer does ONE thing
- ✅ Easy to test each piece
- ✅ Change one layer without breaking others

### Pattern 2: Type-Safe Command Parsing 🔍

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

## 5. Live Coding: Add Your Own Command
**Duration: 3 minutes**

**Let's build a "Hello World" command in real-time!**

### Step 1: Define the Command (30 seconds)
```csharp
public record HelloCliCommand(string Name) : CliCommand;
```

### Step 2: Write the Handler (60 seconds)
```csharp
public class HelloCliCommandHandler 
    : IRequestHandler<HelloCliCommand, CliCommandOutcome[]>
{
    public async Task<CliCommandOutcome[]> Handle(
        HelloCliCommand command, 
        CancellationToken ct)
    {
        var greeting = $"Hello, {command.Name}! Welcome to SpendfulnessCli!";
        return OutcomeAs(greeting);
    }
}
```

### Step 3: Register It (30 seconds)
```csharp
services.AddKeyedTransient<ICliCommandGenerator>(
    "hello",
    (sp, key) => new HelloCliCommandGenerator()
);
```

### Step 4: Run It! (30 seconds)
```bash
$ /hello --name "Joshua"
> Hello, Joshua! Welcome to SpendfulnessCli!
```

**That's it!** Command added in 2.5 minutes. The framework handles:
- ✅ Parsing `/hello --name "Joshua"`
- ✅ Converting `"Joshua"` to a typed string argument
- ✅ Routing to your handler
- ✅ Displaying the output
- ✅ Error handling

---

## 6. Q&A
**Duration: 2 minutes**

### Common Questions

**Q: Can I really integrate ChatGPT with my finances?**
- **A:** Yes! The `/chat` command uses OpenAI API with your transaction data preloaded into a vector database. It's like having an AI financial advisor that knows YOUR spending habits.

**Q: Is this production-ready?**
- **A:** It's a real tool being used for real financial management! The architecture is solid, tested, and documented with ADRs.

**Q: How hard is it to add a command?**
- **A:** You just saw it — about 2 minutes if you know what you want to build. The framework does the heavy lifting.

**Q: What's the catch?**
- **A:** It's a CLI, so no fancy UI. But if you love terminals (and who doesn't?), you'll love this.

**Q: Can I use this framework for my own CLI app?**
- **A:** Absolutely! The core `Cli.*` projects are reusable. The `SpendfulnessCli.*` projects are domain-specific, but the framework is generic.

---

## Summary: The Big Ideas

### What Makes SpendfulnessCli Cool?

1. **Useful Features** 🎯
   - Spare money calculator
   - AI-powered insights
   - Recurring transaction detection
   - Personal inflation tracking
   - Composable command pipelines

2. **Smart Architecture** 🏗️
   - Three-layer separation
   - Type-safe parsing
   - Plugin-based extensibility
   - Reusable aggregations
   - Command pipelines

3. **Maintainable Code** 💎
   - SOLID principles throughout
   - DRY via aggregators and base classes
   - YAGNI keeps it simple
   - Easy to add features (2-minute commands!)
   - Well-documented with ADRs

### Key Takeaways

- ✅ **Good architecture enables cool features** — it's not just theory
- ✅ **SOLID, DRY, YAGNI are practical tools** — not just buzzwords
- ✅ **Build what you need today** — not what you might need tomorrow
- ✅ **Make it easy to add features** — 2-minute commands prove it works
- ✅ **Document your decisions** — ADRs explain the "why"

### Want to Explore More?

**Check out the repo:**
- `/ADR` - Architecture Decision Records explaining design choices
- `CONCEPTS.md` - High-level concepts and patterns
- Test projects - Examples of testing approach
- Try adding your own command!

---

## Thank You! 🎉

**Questions? Let's discuss!**

*"Great architecture is invisible—you only notice it when it's missing."*

**Repository:** https://github.com/KitCli/SpendfulnessCli
**Fun fact:** This entire presentation covers real code from a real project. Every example is authentic!

---

## Resources

### In the Repository
- **ADRs** - Read the architecture decisions that shaped this project
- **CONCEPTS.md** - High-level overview of patterns used
- **Tests** - See how everything is tested
- **Commands** - Explore the 50+ commands

### External Learning
- **Clean Architecture** by Robert C. Martin
- **Domain-Driven Design** by Eric Evans
- **Refactoring** by Martin Fowler
- **MediatR** - https://github.com/jbogard/MediatR

### Try It Yourself!
1. Clone the repository
2. Run `/database create`
3. Add your YNAB API key
4. Explore the commands
5. Add your own command!

**Good luck building maintainable software! 🚀**
