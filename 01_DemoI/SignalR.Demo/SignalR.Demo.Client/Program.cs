using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SignalR.Demo.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome, to whom am I speaking?");
            var username = Console.ReadLine();

            Console.WriteLine($"{Environment.NewLine}Welcome {username} 🤗");
            Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}");

            // Hub Connection
            HubConnection connection = new HubConnectionBuilder()
                                            .WithUrl("https://localhost:5001/chathub")
                                            .Build();

            await connection.StartAsync();

            // Handle new chat messages
            connection.On<string>("NewMessage", (jsonChatMessage) =>
            {
                var chatMessage = JsonSerializer.Deserialize<ChatMessage>(jsonChatMessage);
                Console.WriteLine($"{chatMessage.Username}: {chatMessage.Message}");
                Console.WriteLine($"{chatMessage.Timestamp}");
                Console.WriteLine($"{Environment.NewLine}---{Environment.NewLine}");
            });

            bool userIsActive = true;
            while (userIsActive)
            {
                var messageContent = Console.ReadLine();
                userIsActive = (messageContent != ":!q");
                if (!userIsActive) continue;

                // create chat message
                var chatMessage = new ChatMessage { Username = username, Message = messageContent, Timestamp = DateTimeOffset.Now };
                // serialize message
                var jsonChatMessage = JsonSerializer.Serialize(chatMessage);
                // send message
                await connection.SendAsync("SendMessage", jsonChatMessage);

                Console.WriteLine($"{Environment.NewLine}---{Environment.NewLine}");
            }
        }
    }

    // DTO
    public class ChatMessage
    {
        public string Username { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.MinValue;
    }

}
