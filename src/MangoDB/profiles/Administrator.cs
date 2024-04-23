namespace MangoDB;

public class Administrator
{
    public static void HomePage()
    {
        ScrollingMenu homeMenu =
            new("Please choose an action:", choices: ["Create a new user", "Log out & Exit"]);
        Window.AddElement(homeMenu);

        while (true)
        {
            Window.ActivateElement(homeMenu);
            var homeMenuResponse = homeMenu.GetResponse();

            switch (homeMenuResponse!.Value)
            {
                case 0:
                    Flow.CreateCustomer();
                    break;
                case 1:
                    Dialog logoutDialog =
                        new(["Are you sure you want to log out and exit?"], "No", "Yes");
                    Window.AddElement(logoutDialog);
                    Window.ActivateElement(logoutDialog);
                    var logoutResponse = logoutDialog.GetResponse();
                    if (logoutResponse!.Value == DialogOption.Right)
                    {
                        return;
                    }
                    break;
            }
        }
    }
}
