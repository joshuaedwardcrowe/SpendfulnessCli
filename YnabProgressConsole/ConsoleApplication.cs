using MediatR;
using Microsoft.Extensions.DependencyInjection;
using YnabProgressConsole.Commands;
using YnabProgressConsole.Instructions;

namespace YnabProgressConsole;

public class ConsoleApplication(IServiceProvider serviceProvider)
{
    public async Task Run()
    {
        PrintToConsole("Welcome to YnabProgressConsole!");

        var instructionParser = serviceProvider.GetRequiredService<InstructionParser>();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        while (true)
        {
            PrintToConsole("Enter a Command:");

            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                PrintToConsole("Please enter a Command:");
                continue;
            }

            var instruction = instructionParser.Parse(input);
            if (instruction.Prefix is null)
            {
                PrintToConsole("Commands must have a '/' prefix.");
                continue;
            }

            var generator = serviceProvider.GetKeyedService<ICommandGenerator>(instruction.Name);
            if (generator == null)
            {
                PrintToConsole("Invalid Command, Try Again...");
                continue;
            }
            
            var command = generator.Generate(instruction.Arguments.ToList());

            var table = await mediator.Send(command);

            PrintToConsole(table.ToString());
        }
    }

    private static void PrintToConsole(string print)
    {
        Console.WriteLine(print);
        Console.WriteLine();
    }
}