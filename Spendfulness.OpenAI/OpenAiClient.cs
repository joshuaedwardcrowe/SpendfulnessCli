using System.ClientModel;
using Azure.AI.OpenAI;
using Microsoft.Agents.AI;
using OpenAI.Chat;

namespace Spendfulness.OpenAI;

public class SpendfulnessOpenAiClientBuilder
{
    private Uri _endpointUri;
    private ApiKeyCredential _credential;
    private string _modelName;
    private string _agentPurpose;
    
    public SpendfulnessOpenAiClientBuilder WithSettings(OpenAiSettings settings)
    {
        _endpointUri = new Uri(settings.EndpointUrl);
        _credential = new ApiKeyCredential(settings.ApiKey);
        _modelName = settings.ModelName;
        return this;
    }
    
    public SpendfulnessOpenAiClientBuilder WithAgentPurpose(string agentPurpose)
    {
        _agentPurpose = agentPurpose;
        return this;
    }
    
    public ChatClientAgent Build()
    {
        var agent = new AzureOpenAIClient(_endpointUri, _credential);
        
        var chatClient = agent.GetChatClient(_modelName);
        
        return chatClient.AsAIAgent(_agentPurpose);
    }
}