namespace MangoDB;

public static class Navigation
{
    public static Profile user = Profile.None;

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
        switch (user)
        {
            case Profile.MangoManager:
                MangoManager.HomePage();
                break;
            case Profile.MangoChef:
                // TODO: Add MangoChef method
                break;
            case Profile.Customer:
                Customer.HomePage();
                break;
            case Profile.Supplier:
                // TODO: Add Supplier method
                break;
            default:
                return;
        }
    }
}
