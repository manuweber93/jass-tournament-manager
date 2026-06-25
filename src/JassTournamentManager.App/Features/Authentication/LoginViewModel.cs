using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JassTournamentManager.App.Features.Authentication.Models;
using JassTournamentManager.App.Resources.Localization;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JassTournamentManager.App.Features.Authentication
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        //[NotifyPropertyChangedFor(nameof(IsPreviousParticipationQuestionVisible))]
        //[NotifyPropertyChangedFor(nameof(IsClaimableUserSelectionVisible))]
        //[NotifyPropertyChangedFor(nameof(IsRegistrationFormVisible))]
        public partial RegisterStep CurrentRegisterStep { get; set; } = RegisterStep.PreviousParticipationQuestion;

        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Password { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string FirstName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string LastName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string RegisterEmail { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string RegisterPassword { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ConfirmPassword { get; set; } = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLoginMode))]
        public partial bool IsRegisterMode { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RegisterButtonText))]
        public partial bool WantsToClaimExistingUser { get; set; }

        [ObservableProperty]
        public partial ClaimableUserListItem? selectedClaimableUser {  get; set; }

        public LoginViewModel()
        {
            ClaimableUsers.Add(new(Guid.NewGuid(), "Max", "Muster"));
            ClaimableUsers.Add(new(Guid.NewGuid(), "Maja", "Meisterhans"));
            ClaimableUsers.Add(new(Guid.NewGuid(), "Seraina", "Huber"));
        }

        public bool IsLoginMode => !IsRegisterMode;

        public ObservableCollection<ClaimableUserListItem> ClaimableUsers { get; } = new();

        public string RegisterButtonText => WantsToClaimExistingUser ? AppResources.Register_ClaimAccount : AppResources.Register_CreateAccount;

        [RelayCommand]
        private void ShowLogin()
        {
            IsRegisterMode = false;
        }

        [RelayCommand]
        private void ShowRegister()
        {
            IsRegisterMode = true;
        }

        [RelayCommand]
        private void Login()
        {
            // Later: call login endpoint.
        }

        [RelayCommand]
        private void Register()
        {
            // Later: call register endpoint.
        }
    }
}
