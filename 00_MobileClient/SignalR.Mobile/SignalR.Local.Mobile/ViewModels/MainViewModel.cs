using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SignalR.Local.Mobile.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private readonly Random _randomizer = new Random();

        public MainViewModel()
        {
            var hasValidUsername = this.WhenAnyValue(vm => vm.Username)
                                                        .Select(u => !string.IsNullOrEmpty(u));
            ExecuteGenerateUsername = ReactiveCommand.Create(GenerateUsername);
            ExecuteUsername = ReactiveCommand.CreateFromTask(GoToChat, hasValidUsername);
        }

        [Reactive] public string Username { get; set; } = string.Empty;
        public ICommand ExecuteGenerateUsername { get; set; }
        public ICommand ExecuteUsername { get; set; }
        public Func<string, Task> NavigateToChat { get; set; } = s => Task.CompletedTask;

        private Task GoToChat() => NavigateToChat(Username);

        private void GenerateUsername()
        {
            string GetRandomUser() =>
                "User" + _randomizer.Next(100, 1000);

            Username = GetRandomUser();
            ExecuteUsername.Execute(null);
        }
    }
}