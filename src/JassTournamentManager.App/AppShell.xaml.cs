using JassTournamentManager.App.Features.Authentication;

namespace JassTournamentManager.App;

public partial class AppShell : Shell
{
	public AppShell(LoginPage loginPage)
	{
		InitializeComponent();

        Items.Add(new ShellContent
        {
            Title = "Login",
            Route = "Login",
            Content = loginPage
        }); 
    }
}
