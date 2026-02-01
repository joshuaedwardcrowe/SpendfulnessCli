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

