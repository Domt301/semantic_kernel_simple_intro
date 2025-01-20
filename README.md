# SimpleIntro Application

## Overview

The SimpleIntro application is a console-based chat application that leverages OpenAI's GPT-4 model to provide interactive chat capabilities. The application uses the Microsoft Semantic Kernel to integrate with OpenAI's API and supports plugins for additional functionalities.

## Features

- Interactive chat with OpenAI's GPT-4 model.
- Supports streaming chat message contents.
- Includes plugins for news updates and data archiving.

## Prerequisites

- .NET 9.0 SDK
- OpenAI API key

## Setup

1. Clone the repository.
2. Navigate to the project directory.
3. Ensure you have the .NET 9.0 SDK installed.
4. Create an 

appsettings.json

 file in the project directory with the following content:

```json
{
    "OpenAI": {
      "ModelName": "gpt-4-turbo-preview",
      "ApiKey": "your-openai-api-key"
    }
}
```

Replace `your-openai-api-key` with your actual OpenAI API key.

## Running the Application

1. Open a terminal and navigate to the project directory.
2. Run the application using the following command:

```sh
dotnet run
```

3. The application will prompt you to enter a message. Type your message and press Enter.
4. The application will display the response from the OpenAI model.

## Plugins

### NewsPlugin

Provides news updates.

### ArchivePlugin

Archives data to a file on your computer.

## Code Structure

- 

Program.cs

: Main entry point of the application.
- 

appsettings.json

: Configuration file for OpenAI API settings.
- 

NewsPlugin.cs

: Implementation of the news plugin.
- 

ArchivePlugin.cs

: Implementation of the archive plugin.

## License

This project is licensed under the MIT License. See the LICENSE file for details.