using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.Extensions.Options;
using Spendfulness.OpenAI;

namespace SpendfulnessCli.Commands.Chat.Chat;

public class ChatCliCommandHandler(IOptions<OpenAiSettings> openAiSettings)
    : CliCommandHandler, ICliCommandHandler<ChatCliCommand>
{


    public async Task<CliCommandOutcome[]> Handle(ChatCliCommand request, CancellationToken cancellationToken)
    {
        var agent = new SpendfulnessOpenAiClientBuilder()
            .WithSettings(openAiSettings.Value)
            .WithAgentPurpose("Be a financial advisor")
            .Build();

        var response = await agent.RunAsync(request.Prompt, cancellationToken: cancellationToken);

        return OutcomeAs(response.Text);
    }
}

