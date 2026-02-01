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

