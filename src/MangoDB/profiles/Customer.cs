namespace MangoDB;

public class Customer
{
    public static void HomePage()
    {
        ScrollingMenu homeMenu =
            new(
                "Please choose an action:",
                choices:
                [
                    "Make order",
                    "Past orders",
                    "See profile",
                    "Update profile",
                    "Log out & Exit"
                ]
            );
        Window.AddElement(homeMenu);

        while (true)
        {
            Window.ActivateElement(homeMenu);
            var homeMenuResponse = homeMenu.GetResponse();

            switch (homeMenuResponse!.Value)
            {
                case 0:
                    // Todo
                    break;

                case 1:
                    // Todo
                    break;

                case 2:
                    // Todo
                    break;

                case 3:
                    // Todo
                    break;

                case 4:
                    Dialog logoutDialog =
                        new(["Are you sure you want to log out and exit?"], "No", "Yes");
                    Window.AddElement(logoutDialog);
                    Window.ActivateElement(logoutDialog);
                    var logoutResponse = logoutDialog.GetResponse();
                    Window.RemoveElement(logoutDialog);
                    if (logoutResponse!.Value == DialogOption.Right)
                    {
                        Window.RemoveElement(homeMenu);
                        return;
                    }
                    break;
            }
        }
    }
}
