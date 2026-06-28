namespace JassTournamentManager.App.Features.Authentication
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(AuthenticationViewModel authenticationViewModel)
        {
            InitializeComponent();
            BindingContext = authenticationViewModel;
        }
    }
}
