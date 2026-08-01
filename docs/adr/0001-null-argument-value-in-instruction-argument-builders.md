# Allowing `ArgumentValue` To Default

## Premise
There are situations where I'd like the `ArgumentValue` of an `InstructionArgument` to default - such as when using `bool` arguments. 

For example, when using an argument devised to identify conditional state of the operation, I would like it to default.

e.g. 
```csharp
/spare-money --minus-savings
```

Rather than:
```csharp
/spare-money --minus-savings true
```

## Problem
Currently, the `InstructionTokenParser` throws an exception when we don't pass that `true` in rhe example - it must be there.

While its simple enough to implement this in the `InstructionTokenParser`, it raises a couple questions.

- Should `ArgumentValue` ever allowed to be null so that it can default?
- Do all the the `InstructionArgumentBuilder` implementations need to defautl?
- If not, should this be an excpetional state? Where and how would this be handled?

## Solution
The solution for this problem is composed of two parts:

1. Enabling the value to be passed in as null.
2. Equipping the `InstructionArgumentBuilder` implementations to handle a null argument value token.

### Enabling `null` ArgumentValue

To ensure the `InstructionTokenParser` handled `null` argument value tokens, I stopped assuming that there would always be a ` ` (space) in the input for an argument after the  argument name token:

```csharp
<prefix token>       <command name token>       <argument name token>     <argument value token>
/                    spare-money                --minus-savings           true
```

Then, allowing the argument to be recorded in the `Dictionary<string, string>` buy allowing that value to be `?` nullable.

### Equipping the Builders to Handle Null Argument Values
Given the argument value token can now be `null`, it places a responsibility on all implementations of `IInstructionArgumentBuilder` to handle situation where this is the case when both understanding if the builder is `.For()` building that kind of argument, and how it can `.Create()` that argument if it is `null`.

In the case of implementations, such as `bool`, that have a sensible default (false), we can make sure a return is respected regardless, but for those that do not have a sensible default such as `DateTime`*, somwething else is needed to control flow.

For now, this is going to be handled by treating the lack of an argument value token provided to a `IInstructionArgumentBuilder.Create` call as an exceptional circumstance if the `IInstructionArgumentBuilder` has been designed without a reasonable default value to replace with the lack of that value token.

## Constraints

### Builder .For Must Avoid Exceptional Circumstance
As long as the `.For` call immediately returns `false` for a `null` argument value token, this exceptional circumstance should never actualise.

### Bool Builder Last to Be Injected
Another sensible move in the future would be to remove the dependency on DI injection order for the `BoolInstructionArgumentBuilder` to be the fallback if all other implementations of `IInstructionArgumentBuilder` aren't applicable to the respective argument value token.

## Questions & Answers

### Could the implementation of `IInstructionArgumentBuilder` for `DateTime` be defaulted to `DateTime.Now`?

It could, but you'd get a lot of data sets being reduced down to data that applied today, and would cause a confusing set of results for many commands. This isn't really an acceptable alternative.

### How could the dependency on `IInstructionArgumentBuilder` implementations to protect the caller from exceptional circumstances be removed?
It is sensible in future to make sure that individual implementations aren't liable for upholding this constraint, perhaps through the base abstraction fulfilling the contract for the sake of this constraint and delegating to the implementations for the real specifics.

### How could the dependency on order of DI injection for `IInstructionArgumentBuilder` be resolved?
Perhaps this could be achieved by implementing some kind of InstructionArgumentFactory that uses these `IInstructionArgumentBuilder` implementations, and treats the `BoolInstructionArgumentBuilder` as the default, injecting that as a singleton in its own right so it doesn't get folded into the `IInstructionArgumentBuilder` implementations collection.
