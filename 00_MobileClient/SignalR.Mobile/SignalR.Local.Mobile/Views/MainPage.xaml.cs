using System.Threading.Tasks;
using SignalR.Local.Mobile.ViewModels;
using Xamarin.Forms;

namespace SignalR.Local.Mobile.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel = new MainViewModel();
        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.NavigateToChat = NavigateToChat;
        }

        private Task NavigateToChat(string username) => Navigation.PushAsync(new ChatPage(username));

    }
}