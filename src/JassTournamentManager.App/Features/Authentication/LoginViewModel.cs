using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JassTournamentManager.App.Features.Authentication.Models;
using JassTournamentManager.App.Resources.Localization;
using System.Collections.ObjectModel;

namespace JassTournamentManager.App.Features.Authentication
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly List<ClaimableUserListItem> allClaimableUsers = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsPreviousParticipationQuestionVisible))]
        [NotifyPropertyChangedFor(nameof(IsClaimableUserSelectionVisible))]
        [NotifyPropertyChangedFor(nameof(IsRegistrationFormVisible))]
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
        public partial string RegisterPassword { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ConfirmPassword { get; set; } = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLoginMode))]
        public partial bool IsRegisterMode { get; set; }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(RegisterButtonText))]
        public partial bool WantsToClaimExistingUser { get; set; }

        public ObservableCollection<ClaimableUserListItem> FilteredClaimableUsers { get; } = new();

        [ObservableProperty]
        public partial string ClaimableUsersFilterText { get; set;  } = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ContinueWithSelectedClaimableUserCommand))]

        public partial ClaimableUserListItem? SelectedClaimableUser { get; set; }

        public LoginViewModel()
        {
            LoadDummyClaimableUsers();
        }

        public bool IsLoginMode => !IsRegisterMode;

        public bool IsPreviousParticipationQuestionVisible => CurrentRegisterStep == RegisterStep.PreviousParticipationQuestion;

        public bool IsClaimableUserSelectionVisible => CurrentRegisterStep == RegisterStep.ClaimableUserSelection;

        public bool IsRegistrationFormVisible => CurrentRegisterStep == RegisterStep.RegistrationForm;

        public string RegisterButtonText => WantsToClaimExistingUser ? AppResources.Register_ClaimAccount : AppResources.Register_CreateAccount;

        partial void OnClaimableUsersFilterTextChanged(string value)
        {
            SelectedClaimableUser = null;
            ApplyClaimableUserFilter();
        }

        [RelayCommand]
        private void ShowLogin()
        {
            IsRegisterMode = false;
        }

        [RelayCommand]
        private void ShowRegister()
        {
            IsRegisterMode = true;
            CurrentRegisterStep = RegisterStep.PreviousParticipationQuestion;
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

        [RelayCommand]
        private void RegisterNavigateBack()
        {
            if (CurrentRegisterStep == RegisterStep.ClaimableUserSelection)
            {
                CurrentRegisterStep = RegisterStep.PreviousParticipationQuestion;
            }

            if (CurrentRegisterStep == RegisterStep.RegistrationForm)
            {
                if (SelectedClaimableUser is not null)
                {
                    CurrentRegisterStep = RegisterStep.ClaimableUserSelection;
                } else
                {
                    CurrentRegisterStep = RegisterStep.PreviousParticipationQuestion;
                }
            }
        }

        [RelayCommand]
        private void ShowClaimableUserSelection()
        {
            CurrentRegisterStep = RegisterStep.ClaimableUserSelection;
        }

        [RelayCommand]
        private void NoUserClaiming()
        {
            SelectedClaimableUser = null;
            CurrentRegisterStep = RegisterStep.RegistrationForm;
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        [RelayCommand(CanExecute = nameof(CanContinueWithSelectedClaimableUser))]
        private void ContinueWithSelectedClaimableUser()
        {
            if (SelectedClaimableUser is null)
            {
                return;
            }

            PrefillRegistrationForm(SelectedClaimableUser);
            CurrentRegisterStep = RegisterStep.RegistrationForm;
        }

        private void LoadDummyClaimableUsers()
        {
            allClaimableUsers.Clear();

            allClaimableUsers.Add(new(Guid.NewGuid(), "Max", "Muster"));
            allClaimableUsers.Add(new(Guid.NewGuid(), "Maja", "Meisterhans"));
            allClaimableUsers.Add(new(Guid.NewGuid(), "Seraina", "Huber"));

            ApplyClaimableUserFilter();
        }

        private void ApplyClaimableUserFilter()
        {
            string filterValue = ClaimableUsersFilterText.Trim();

            IEnumerable<ClaimableUserListItem> filteredUsers = allClaimableUsers.OrderBy(user => user.DisplayName);
            if (!string.IsNullOrWhiteSpace(filterValue))
            {
                filteredUsers = filteredUsers.Where(user =>
                    user.DisplayName.Contains(filterValue, StringComparison.CurrentCultureIgnoreCase));
            }

            FilteredClaimableUsers.Clear();

            foreach (ClaimableUserListItem user in filteredUsers)
            {
                FilteredClaimableUsers.Add(user);
            }
        }

        private bool CanContinueWithSelectedClaimableUser()
        {
            return SelectedClaimableUser is not null;
        }

        private void PrefillRegistrationForm(ClaimableUserListItem selectedClaimableUser)
        {
            FirstName = selectedClaimableUser.FirstName;
            LastName = selectedClaimableUser.LastName;
        }
    }
}
