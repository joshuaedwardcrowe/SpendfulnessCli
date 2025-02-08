using MediatR;
using Microsoft.Extensions.DependencyInjection;
using YnabProgressConsole.Commands;
using YnabProgressConsole.Instructions;

namespace YnabProgressConsole;

public class ConsoleApplication(IServiceProvider serviceProvider)
{
    public async Task Run()
    {
        Console.WriteLine("Welcome to YnabProgressConsole!");
        
        var instructionParser = serviceProvider.GetService<InstructionParser>();
        if (instructionParser == null)
        {
            Console.WriteLine("No instruction parser registered.");
        }
        
        var mediator = serviceProvider.GetService<IMediator>();
        if (mediator is null)
        {
            Console.WriteLine("No mediator registered.");
        }
        
        while (true)
        {
            Console.WriteLine("Enter a Command:");
            
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please enter a Command:");
                continue;
            }
            
            var instruction = instructionParser.Parse(input);
            
            var commandGenerator = serviceProvider.GetKeyedService<ICommandGenerator>(instruction.InstructionName);
            if (commandGenerator is null)
            {
                Console.WriteLine("Invalid Command");
                continue;
            }
            
            var command = commandGenerator.Generate(instruction.Arguments.ToList());
            
            var table = await mediator.Send(command);
            
            Console.WriteLine(table);
        }
    }
    
}