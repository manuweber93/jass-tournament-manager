using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace JassTournamentManager.App.Features.Authentication
{
    public partial class LoginFormViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Password { get; set; } = string.Empty;

        public LoginFormViewModel()
        {

        }

        [RelayCommand]
        private void Login()
        {
            // Later: call login endpoint.
        }
    }
}
