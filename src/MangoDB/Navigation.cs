namespace MangoDB;

public static class Navigation
{
    public static Profile UserProfile = Profile.None;
    public static string UserEmail = "";

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
                // TODO: Add MangoChef method
                Core.WriteDebugMessage(lines: "MangoChef method not implemented yet.");
                Window.Freeze();
                break;
            case Profile.Customer:
                Customer.HomePage();
                break;
            case Profile.Supplier:
                // TODO: Add Supplier method
                Core.WriteDebugMessage(lines: "Supplier method not implemented yet.");
                Window.Freeze();
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
