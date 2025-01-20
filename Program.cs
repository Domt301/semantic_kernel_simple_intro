using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.IO;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var modelName = configuration["OpenAI:ModelName"];
var apiKey = configuration["OpenAI:ApiKey"];

var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion(modelName, apiKey);
Kernel kernel = builder.Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();

ChatHistory chatMessages = new ChatHistory();

while (true){
    Console.Write("Prompt: ");
    chatMessages.AddUserMessage(Console.ReadLine());
    var completion = chatService.GetStreamingChatMessageContentsAsync(chatMessages, kernel: kernel);

    string fullMessage = "";
    await foreach (var content in completion){
        Console.Write(content.Content);
        fullMessage += content.Content;
    }
    chatMessages.AddAssistantMessage(fullMessage);
    Console.WriteLine();
}