using System.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using SignalR.Local.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SignalR.Local.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage, IViewFor<ChatViewModel>
    {
        public ChatPage(string username)
        {
            InitializeComponent();
            BindingContext = ViewModel;
            ViewModel.Username = username;
            // Navigation.RemovePage(Navigation.NavigationStack.First());
            this.WhenActivated((disposable) => { });
        }

        public ChatViewModel? ViewModel { get; set; } = new ChatViewModel();
        object? IViewFor.ViewModel { get => ViewModel; set => ViewModel = value as ChatViewModel; }

        //object IViewFor.ViewModel
        //{
        //    get => ViewModel;
        //    set => ViewModel = value as ChatViewModel;
        //}

        //public ChatViewModel ViewModel
        //{
        //    get; set;
        private class Gnabber : ReactiveContentPage<ChatViewModel> { }
    }
}