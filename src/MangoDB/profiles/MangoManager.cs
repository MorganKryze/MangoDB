namespace MangoDB;

public class MangoManager
{
    public static void HomePage()
    {
        ScrollingMenu homeMenu =
            new(
                "Please choose an action:",
                choices: ["View customers", "Create a new customer", "Hash text", "Log out & Exit"]
            );
        Window.AddElement(homeMenu);

        while (true)
        {
            Window.ActivateElement(homeMenu);
            var homeMenuResponse = homeMenu.GetResponse();

            switch (homeMenuResponse!.Value)
            {
                case 0:
                    bool userView = Flow.ViewCustomersTable();
                    if (!userView)
                    {
                        Dialog userViewDialog =
                            new(
                                [
                                    "User view cancelled",
                                    string.Empty,
                                    "The user view process was cancelled.",
                                    "You may try again later."
                                ],
                                rightOption: "OK"
                            );
                        Window.AddElement(userViewDialog);
                        Window.ActivateElement(userViewDialog);

                        Window.RemoveElement(userViewDialog);
                    }
                    break;

                case 1:
                    bool userCreation = Flow.CreateCustomer();
                    if (!userCreation)
                    {
                        Dialog userCreationDialog =
                            new(
                                [
                                    "User creation cancelled",
                                    string.Empty,
                                    "The user creation process was cancelled.",
                                    "You may try again later."
                                ],
                                rightOption: "OK"
                            );
                        Window.AddElement(userCreationDialog);
                        Window.ActivateElement(userCreationDialog);

                        Window.RemoveElement(userCreationDialog);
                    }
                    break;

                case 2:
                    bool hashTranslation = Flow.ShowHashed();
                    if (!hashTranslation)
                    {
                        Dialog hashTranslationDialog =
                            new(
                                [
                                    "Hash translation cancelled",
                                    string.Empty,
                                    "The hash translation process was cancelled.",
                                    "You may try again later."
                                ],
                                rightOption: "OK"
                            );
                        Window.AddElement(hashTranslationDialog);
                        Window.ActivateElement(hashTranslationDialog);

                        Window.RemoveElement(hashTranslationDialog);
                    }
                    break;

                case 3:
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
