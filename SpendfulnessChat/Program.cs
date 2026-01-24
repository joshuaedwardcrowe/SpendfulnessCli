using System.ClientModel;
using Azure.AI.OpenAI;
using OpenAI.Chat;

var foundryUrl = "https://spendfulness-chat-foundry-eus-2.openai.azure.com/";

var uri = new Uri(foundryUrl);

var credential = new ApiKeyCredential("key here");

var agent = new AzureOpenAIClient(uri, credential);

var modelName = "spendfulness-chat-gpt-4o-mini-eus-2";

var chatClient = agent.GetChatClient(modelName);

var jokeChatClient = chatClient.AsAIAgent("You are good at telling jokes.");

var answer = await jokeChatClient.RunAsync("Tell me a joke about a pirate");

Console.WriteLine(answer);