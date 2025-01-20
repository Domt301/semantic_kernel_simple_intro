using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.EntityFrameworkCore;
using SimpleIntro;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var modelName = configuration["OpenAI:ModelName"];
var apiKey = configuration["OpenAI:ApiKey"];
var connectionString = configuration["Database:ConnectionString"];
var builder = Kernel.CreateBuilder();
builder.AddOpenAIChatCompletion(modelName, apiKey);

// Add database context and chat persistence service
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<IChatPersistenceService, ChatPersistenceService>();

builder.Plugins.AddFromType<NewsPlugin>();
builder.Plugins.AddFromType<ArchivePlugin>();

Kernel kernel = builder.Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();
var persistenceService = kernel.GetRequiredService<IChatPersistenceService>();

// Session management
Console.Write("Enter session ID (or press Enter for new session): ");
string sessionId = Console.ReadLine();

if (string.IsNullOrEmpty(sessionId))
{
    sessionId = await persistenceService.CreateNewSessionAsync();
    Console.WriteLine($"Created new session: {sessionId}");
}

// Load existing chat history if available
ChatHistory chatMessages = await persistenceService.LoadChatHistoryAsync(sessionId);

while (true)
{
    Console.Write("Prompt: ");
    var userInput = Console.ReadLine();
    
    if (string.IsNullOrEmpty(userInput))
        continue;

    try 
    {
        // Save user message
        await persistenceService.SaveMessageAsync(sessionId, new ChatMessageDto
        {
            Role = "user",
            Content = userInput
        });
        
        chatMessages.AddUserMessage(userInput);
        
        var completion = chatService.GetStreamingChatMessageContentsAsync(
            chatMessages,
            executionSettings: new OpenAIPromptExecutionSettings()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            },
            kernel: kernel);

        try
        {
            string fullMessage = "";
            await foreach (var content in completion)
            {
                if (content.Content != null)
                {
                    Console.Write(content.Content);
                    fullMessage += content.Content;
                }
            }
            
            if (!string.IsNullOrEmpty(fullMessage))
            {
                await persistenceService.SaveMessageAsync(sessionId, new ChatMessageDto
                {
                    Role = "assistant",
                    Content = fullMessage.Trim()
                });
                
                chatMessages.AddAssistantMessage(fullMessage.Trim());
            }
            else
            {
                Console.WriteLine("\nWarning: Empty response received from assistant");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError processing assistant message: {ex.Message}");
            continue;
        }

        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        continue;
    }
}