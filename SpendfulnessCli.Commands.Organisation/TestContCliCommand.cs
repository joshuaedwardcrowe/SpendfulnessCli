using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Organisation;

public record TestContCliCommand : ContinuousCliCommand
{
    
}

public class TestContCliCommandGenerator : ICliCommandGenerator<TestContCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
    {
        return new TestContCliCommand();
    }

    public void NextCommands(CliNextCommmandDefinition commmandDefinition)
    {
        commmandDefinition
            .Next<TestAfterCliCommand>();
    }
}

public class TestContCliCommandHandler : ICliCommandHandler<TestContCliCommand>
{
    public Task<CliCommandOutcome> Handle(TestContCliCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine("TestContCommand executed.");
        
        var properties = new List<CliCommandProperty>
        {
            new TestContCommandProperty("test", "value")
        };
        
        var outcome = new CliCommandPropertiesOutcome(properties);
        return Task.FromResult<CliCommandOutcome>(outcome);
    }
}

public class TestContCommandProperty(string propertyKey, string propertyValue) : CustomCliCommandProperty<string>(propertyKey, propertyValue)
{
    
}

public record TestAfterCliCommand : CliCommand
{
    
}

public class TestAfterCliCommandGenerator : ICliCommandGenerator<TestAfterCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
    {
        return new TestAfterCliCommand();
    }
}

public class TestAfterCliCommandHandler : ICliCommandHandler<TestAfterCliCommand>
{
    public Task<CliCommandOutcome> Handle(TestAfterCliCommand request, CancellationToken cancellationToken)
    {
        Console.WriteLine("TestAfterCommand executed.");
        
        return Task.FromResult<CliCommandOutcome>(new CliCommandNothingOutcome());
    }
}