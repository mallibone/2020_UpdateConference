using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SignalR.Local.Mobile.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace SignalR.Local.Mobile.ViewModels
{
    public class ChatViewModel : ReactiveObject, IActivatableViewModel
    {
        private readonly HubConnection _connection;

        public ChatViewModel()
        {
            Activator = new ViewModelActivator();
            var hasChatMessage = this.WhenAnyValue(vm => vm.ChatMessage)
                                                        .Select(msg => !string.IsNullOrEmpty(msg));
            ExecuteSendMessage = ReactiveCommand.CreateFromTask(SendMessage, hasChatMessage);
            
            _connection = new HubConnectionBuilder()
                // Local
                .WithUrl("https://localhost:5001/ChatHub")
                // WebService
                //.WithUrl("https://stayconnectedwithsignalrwebapp.azurewebsites.net/ChatHub")
                // AF
                // .WithUrl("https://stayconnectedwithsignalrazurefunction.azurewebsites.net/api/")
                .WithAutomaticReconnect()
                .Build();
            
            this.WhenActivated(async (CompositeDisposable disposable) =>
            {
                await _connection.StartAsync();
                
                _connection.On<string>("NewMessage", (jsonChatMessage) =>
                {
                    var chatMessage = JsonConvert.DeserializeObject<ChatMessage>(jsonChatMessage);
                    ChatMessages.Add(chatMessage);
                });
            });
        }

        [Reactive] public string ChatMessage { get; set; } = string.Empty;
        public ObservableCollection<ChatMessage> ChatMessages { get; } = new ObservableCollection<ChatMessage>();
        public ICommand ExecuteSendMessage { get; }
        public ViewModelActivator Activator { get; set; }
        public string Username { get; internal set; } = string.Empty;

        private async Task SendMessage()
        {
            var chatMessage = new ChatMessage
                {Username = Username, Message = ChatMessage, Timestamp = DateTimeOffset.Now};
            var jsonChatMessage = JsonConvert.SerializeObject(chatMessage);
            await _connection.SendAsync("SendMessage", jsonChatMessage);
            ChatMessage = string.Empty;
        }
    }
}