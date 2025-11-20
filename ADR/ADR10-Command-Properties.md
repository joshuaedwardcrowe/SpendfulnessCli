# Command Properties

## Premise

In a CLI workflow, commands often need to operate on data or context produced by previous commands. For example, an aggregation command might produce a dataset that a subsequent display or formatting command needs to consume. The system needs a way to pass this context between commands in a type-safe, extensible manner without tightly coupling commands to each other's implementations.

The system needs to:
- Convert command outcomes into reusable properties
- Pass properties from previous commands to subsequent commands
- Support different types of properties (messages, aggregators, etc.)
- Allow extensibility for new property types
- Maintain type safety throughout the conversion process

## Problem

Managing inter-command communication in a CLI workflow presents several challenges:

1. **Type Safety** - Command outcomes need to be converted to properties without losing type information, enabling type-safe consumption by subsequent commands.

2. **Extensibility** - The system should support adding new types of properties without modifying existing code or breaking the open/closed principle.

3. **Loose Coupling** - Commands shouldn't directly depend on each other's specific outcome types, but should instead work with a common property abstraction.

4. **Discovery** - The system needs to automatically discover which outcomes can be converted to properties and which factory should handle each conversion.

5. **Context Propagation** - Properties from multiple previous commands might need to be available to a single new command, requiring proper aggregation and filtering.

6. **Factory Selection** - Multiple factories exist, and the system must select the correct factory for each outcome type without manual routing logic.

## Solution

The Command Properties Concept implements a factory pattern for converting command outcomes into typed properties that can be consumed by subsequent commands. This creates a flexible, extensible mechanism for inter-command communication.

### Architecture Overview

```
CliCommandOutcome (from previous commands)
    ↓
ICliCommandPropertyFactory.CanCreateProperty() (checks if applicable)
    ↓
ICliCommandPropertyFactory.CreateProperty() (converts to property)
    ↓
CliCommandProperty (abstract base)
    ├── ValuedCliCommandProperty<T> (generic typed property)
    │   ├── MessageCliCommandProperty (string messages)
    │   └── AggregatorCliCommandProperty<T> (typed aggregators)
    ↓
IUnidentifiedCliCommandGenerator.Generate(instruction, properties)
    ↓
CliCommand (uses properties for execution)
```

### 1. CliCommandProperty - The Base Abstraction

```csharp
/// <summary>
/// Defines a property that can be associated with a CLI command.
/// </summary>
public abstract class CliCommandProperty
{
}
```

**Key Design Decision**: The base class is intentionally minimal, serving purely as a marker type. All functionality is added through inheritance, allowing maximum flexibility for different property types.

**Purpose**: Provides a common type that command generators can accept in their `Generate()` method, enabling polymorphic property handling.

### 2. ValuedCliCommandProperty<T> - Typed Properties

```csharp
public class ValuedCliCommandProperty<TCommandPropertyValue>(TCommandPropertyValue value) : CliCommandProperty
{
    public TCommandPropertyValue Value { get; set; } = value;
}
```

**Key Design Decision**: Generic typed properties preserve type information while still inheriting from the base abstraction. This enables type-safe property access after type checking.

**Responsibilities**:
- Store a strongly-typed value
- Provide mutable access to the value via a property
- Bridge between the abstract base and concrete implementations

### 3. Concrete Property Implementations

```csharp
public class MessageCliCommandProperty(string message) : ValuedCliCommandProperty<string>(message)
{
}

public class AggregatorCliCommandProperty<TAggregate>(CliAggregator<TAggregate> value)
    : ValuedCliCommandProperty<CliAggregator<TAggregate>>(value)
{
}
```

**Key Design Decision**: Each concrete property type is a simple wrapper around `ValuedCliCommandProperty<T>`, adding no additional logic. This maintains a clean type hierarchy for pattern matching and type inspection.

**Examples**:
- **MessageCliCommandProperty** - Carries string messages from `CliCommandMessageOutcome`
- **AggregatorCliCommandProperty<T>** - Carries typed aggregators from `CliCommandAggregatorOutcome<T>`

### 4. ICliCommandPropertyFactory - The Conversion Interface

```csharp
public interface ICliCommandPropertyFactory
{
    bool CanCreateProperty(CliCommandOutcome outcome);
    
    CliCommandProperty CreateProperty(CliCommandOutcome outcome);
}
```

**Key Design Decision**: The factory pattern with a two-method interface (`CanCreateProperty` + `CreateProperty`) enables discovery and conversion in a single workflow without exception-based control flow.

**Responsibilities**:
- Determine if a factory can handle a specific outcome type
- Convert an outcome to the appropriate property type
- Throw `InvalidOperationException` if `CreateProperty` is called inappropriately

### 5. Concrete Factory Implementations

```csharp
public class MessageCliCommandPropertyFactory : ICliCommandPropertyFactory
{
    public bool CanCreateProperty(CliCommandOutcome outcome) 
        => outcome is CliCommandMessageOutcome;

    public CliCommandProperty CreateProperty(CliCommandOutcome outcome)
    {
        if (outcome is CliCommandMessageOutcome messageOutcome)
        {
            return new MessageCliCommandProperty(messageOutcome.Message);
        }

        throw new InvalidOperationException("Cannot create property from the given outcome.");
    }
}
```

**Key Design Decision**: Each factory is responsible for one outcome-to-property conversion. Pattern matching (`is` operator) provides type-safe outcome inspection.

**MessageCliCommandPropertyFactory**: Converts `CliCommandMessageOutcome` to `MessageCliCommandProperty`.

```csharp
public class AggregatorCliCommandPropertyFactory<TAggregate> : ICliCommandPropertyFactory
{
    public bool CanCreateProperty(CliCommandOutcome outcome)
    {
        return outcome is CliCommandAggregatorOutcome<TAggregate>;
    }

    public CliCommandProperty CreateProperty(CliCommandOutcome outcome)
    {
        if (outcome is CliCommandAggregatorOutcome<TAggregate> aggregatorOutcome)
        {
            return new AggregatorCliCommandProperty<TAggregate>(aggregatorOutcome.Aggregator);
        }
        
        throw new InvalidOperationException(
            $"Cannot create AggregatorCliCommandProperty<{typeof(TAggregate).Name}> from outcome of type {outcome.GetType().Name}");
    }
}
```

**Key Design Decision**: Generic factory for aggregators. A factory instance exists for each aggregate type, enabling type-safe conversion for different aggregator types.

**AggregatorCliCommandPropertyFactory<T>**: Converts `CliCommandAggregatorOutcome<T>` to `AggregatorCliCommandProperty<T>`.

### 6. CliWorkflowCommandProvider - The Orchestrator

```csharp
public class CliWorkflowCommandProvider(IServiceProvider serviceProvider) : ICliWorkflowCommandProvider
{
    public CliCommand GetCommand(CliInstruction instruction, List<CliCommandOutcome> outcomes)
    {
        var properties = ConvertOutcomesToProperties(outcomes);
        var generator = serviceProvider.GetKeyedService<IUnidentifiedCliCommandGenerator>(instruction.Name);
        return generator.Generate(instruction, properties);
    }

    private List<CliCommandProperty> ConvertOutcomesToProperties(List<CliCommandOutcome> priorOutcomes)
    {
        var propertyFactories = serviceProvider.GetServices<ICliCommandPropertyFactory>();
        
        var convertableOutcomes = priorOutcomes
            .Where(priorOutcome => propertyFactories
                .Any(propertyFactory => propertyFactory.CanCreateProperty(priorOutcome)));
        
        return convertableOutcomes
            .Select(priorOutcome => propertyFactories
                .First(propertyFactory => propertyFactory.CanCreateProperty(priorOutcome))
                .CreateProperty(priorOutcome))
            .ToList();
    }
}
```

**Key Design Decision**: The command provider is responsible for orchestrating the conversion process. It:
1. Retrieves all registered factories from dependency injection
2. Filters outcomes to those that can be converted
3. Selects the appropriate factory for each outcome
4. Converts outcomes to properties
5. Passes properties to command generators

**Responsibilities**:
- Coordinate between outcomes and factories
- Filter convertible outcomes
- Select correct factory for each outcome
- Pass converted properties to command generators

### 7. Command Generator Integration

```csharp
public interface IUnidentifiedCliCommandGenerator
{
    CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties);
}
```

**Key Design Decision**: Command generators accept a list of properties, enabling them to inspect and use relevant properties. Generators can pattern-match or filter properties based on their needs.

**Usage Pattern**:
```csharp
public class SomeCommandGenerator : ICliCommandGenerator<SomeCommand>
{
    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
    {
        var aggregatorProperty = properties
            .OfType<AggregatorCliCommandProperty<MyAggregate>>()
            .FirstOrDefault();
            
        return new SomeCommand(aggregatorProperty?.Value);
    }
}
```

### 8. Dependency Injection Registration

```csharp
public static class CommandPropertyServiceCollectionExtensions
{
    public static IServiceCollection AddCommandProperties(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<ICliCommandPropertyFactory, MessageCliCommandPropertyFactory>();
    }
    
    public static IServiceCollection AddAggregatorCommandPropertiesFromAssembly(
        this IServiceCollection serviceCollection, Assembly? assembly)
    {
        // Scan assembly for CliAggregator<T> implementations
        // Create AggregatorCliCommandPropertyFactory<T> for each aggregate type
        // Register each factory as ICliCommandPropertyFactory
    }
}
```

**Key Design Decision**: Extension methods provide convenient, discoverable registration APIs. The `AddAggregatorCommandPropertiesFromAssembly` method uses reflection to automatically register factories for all aggregator types in an assembly.

## Constraints

### Factory Selection is First-Match

The `ConvertOutcomesToProperties` method uses `.First()` to select a factory, meaning if multiple factories claim they can handle an outcome, the first one wins. This could lead to unexpected behavior if factories are not properly scoped.

### No Property Validation

Command generators receive a `List<CliCommandProperty>` with no guarantees about which properties are present. Generators must handle missing properties gracefully.

### Properties are Mutable

The `ValuedCliCommandProperty<T>.Value` property has a setter, allowing modification after creation. This could lead to unintended side effects if properties are shared between commands.

### No Built-in Property Lifetime Management

Properties exist in a list and are passed to command generators, but there's no automatic cleanup or lifetime management. Properties persist for the life of the list.

### Single Factory Per Outcome

Each outcome can only be converted by one factory. The system selects the first matching factory and doesn't attempt fallback or composition of multiple factories.

### Reflection-based Assembly Scanning

The `AddAggregatorCommandPropertiesFromAssembly` method uses reflection to discover aggregator types. This adds startup time overhead and can be fragile if type structures change.

## Questions & Answers

### Why use a factory pattern instead of a direct mapping?

The factory pattern enables extensibility without modifying existing code. New property types can be added by implementing `ICliCommandPropertyFactory` and registering it in DI, following the open/closed principle.

### Why separate CanCreateProperty and CreateProperty methods?

Separating the check from the conversion allows the orchestrator to filter outcomes before attempting conversion, avoiding exceptions for non-convertible outcomes. It also makes the factory's capabilities explicit.

### Could properties be immutable?

Yes, `ValuedCliCommandProperty<T>.Value` could be made read-only. However, the current design allows command generators to modify properties if needed, providing flexibility for stateful scenarios.

### How does a command generator know which properties are available?

Generators must inspect the properties list using pattern matching (e.g., `.OfType<SomeProperty>()`). There's no static typing or compile-time verification of available properties.

### What happens if no factory can handle an outcome?

The outcome is filtered out in `ConvertOutcomesToProperties` and simply won't appear in the properties list. This is by design—not all outcomes need to become properties.

### Why are aggregator factories generic?

Generic factories preserve type information for aggregators. This enables type-safe access to aggregated data when the property is consumed by command generators.

### How do you add a new property type?

1. Create a new class inheriting from `CliCommandProperty` (typically via `ValuedCliCommandProperty<T>`)
2. Create a factory implementing `ICliCommandPropertyFactory`
3. Register the factory in the DI container
4. Use the property in command generators by filtering the properties list

### Could this pattern be used for command validation?

Not directly. Properties are derived from outcomes (post-execution), not from instructions (pre-execution). Validation would require a different mechanism that operates on instructions.

### What if multiple commands produce the same outcome type?

All outcomes are passed to the conversion process, so multiple outcomes of the same type would each be converted to properties. The properties list could contain duplicates of the same property type.

### Is there a performance impact of using reflection for aggregator registration?

Yes, reflection-based scanning happens at startup. For applications with many aggregator types, this could add noticeable startup time. However, it only occurs once during service registration.
