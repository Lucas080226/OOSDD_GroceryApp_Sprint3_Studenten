using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.App.ViewModels
{
    public partial class RegistratieViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        private readonly GlobalViewModel _global;
        private readonly IClientService _clientService;

        [ObservableProperty]
        private string email = "bob@mail.com";

        [ObservableProperty]
        private string password = "12345678910";

        [ObservableProperty]
        private string registratieMessage;

        public RegistratieViewModel(IAuthService authService, GlobalViewModel global, IClientService clientService)
        {
            _authService = authService;
            _global = global;
            _clientService = clientService;
        }

        [RelayCommand]
        private void Registratie()
        {
            string? errorMessage;
            Client? newClient = _authService.Registratie(Email, Password, out errorMessage);

            if (newClient != null)
            {
                RegistratieMessage = $"Account aangemaakt! Je kunt nu inloggen, {newClient.Name}.";
            }
            else
            {
                RegistratieMessage = errorMessage;
            }
        }


        [RelayCommand]
        private void NavigateToLogin()
        {
            // Naar het Login-scherm navigeren
            var loginViewModel = new LoginViewModel(_authService, _global, _clientService);
            Application.Current.MainPage = new LoginView(loginViewModel);
        }
    }
}
