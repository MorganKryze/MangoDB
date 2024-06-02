namespace MangoDB;

public static class Navigation
{
    private static Profile userProfile = Profile.None;
    private static string userEmail = "";

    public static Profile UserProfile { get => userProfile; set => userProfile = value; }
    public static string UserEmail { get => userEmail; set => userEmail = value; }

    public static void Setup()
    {
        Title title = new("Mango DB", font: Font.Ghost);
        Header header = new("", "Welcome to the MangoDB smoothie shop", "");
        Footer footer = new("[ESC] Back", "[Z|↑] Up   [S|↓] Down", "[ENTER] Select");
        FakeLoadingBar startingLoadingBar = new("[ Starting the app ... ]");
        Dialog errorDialog =
            new(["Error", "Failed to connect to the database"], rightOption: "Exit");

        Window.AddElement(title, header, footer);

        if (!RepositoryImplementation.InitRepository())
        {
            Window.AddElement(errorDialog);
            Window.Render();
            Window.ActivateElement(errorDialog);
            Window.Close();
            return;
        }

        Window.Render();
        Window.AddElement(startingLoadingBar);
        Window.ActivateElement(startingLoadingBar);
    }

    public static void Redirection()
    {
        switch (UserProfile)
        {
            case Profile.MangoManager:
                MangoManager.HomePage();
                break;
            case Profile.MangoChef:
                MangoChef.HomePage();
                break;
            case Profile.Customer:
                Customer.HomePage();
                break;
            case Profile.Supplier:
                Supplier.HomePage();
                break;
            default:
                return;
        }
    }

    public static void LogIn(Profile profile, string email)
    {
        UserProfile = profile;
        UserEmail = email;
    }

    public static void LogOut()
    {
        UserProfile = Profile.None;
        UserEmail = "";
        FakeLoadingBar fakeLoadingBar = new("[ Logging out ... ]");
        Window.AddElement(fakeLoadingBar);
        Window.ActivateElement(fakeLoadingBar);
        Window.RemoveElement(fakeLoadingBar);
    }
}
