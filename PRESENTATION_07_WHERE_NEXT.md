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

