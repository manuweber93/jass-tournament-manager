namespace JassTournamentManager.App;

public partial class App : Application
{
    private readonly IServiceProvider serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        this.serviceProvider = serviceProvider;
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
        AppShell appShell = serviceProvider.GetRequiredService<AppShell>();
        return new Window(appShell);
    }
}