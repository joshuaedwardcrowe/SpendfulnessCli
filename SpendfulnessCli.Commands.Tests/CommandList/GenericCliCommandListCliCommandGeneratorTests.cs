using Cli.Instructions.Abstractions;
using SpendfulnessCli.Commands;
using SpendfulnessCli.Commands.Generators;

namespace Cli.Spendfulness.Commands.Tests.CommandList;

[TestFixture]
public class GenericCliCommandListCliCommandGeneratorTests
{
    [Test]
    public void GivenAnyArguments_WhenGenerate_ReturnsCommandListCommand()
    {
        var generator = new GenericCliCommandListCliCommandGenerator();

        var instruction = new CliInstruction(
            string.Empty,
            string.Empty,
            string.Empty, 
            []);
        
        var result = generator.Generate(instruction);
        
        Assert.That(result, Is.TypeOf<CommandListCliCommand>());
    }
}