namespace MangoDB;

public class Customer
{
    public static void HomePage()
    {
        EmbedText card = Component.GetCustomerCard(Navigation.UserEmail);
        Window.AddElement(card);

        ScrollingMenu homeMenu =
            new(
                "Please choose an action:",
                choices: ["Make order", "Past orders", "Update profile", "Log out"]
            );
        Window.AddElement(homeMenu);

        while (true)
        {
            Window.ActivateElement(card);
            Window.ActivateElement(homeMenu);

            var homeMenuResponse = homeMenu.GetResponse();
            Window.DeactivateElement(card);
            switch (homeMenuResponse!.Value)
            {
                case 0:
                    // Todo
                    break;

                case 1:
                    // Todo
                    break;

                case 2:
                    bool updateLoop = true;
                    while (updateLoop)
                    {
                        Window.ActivateElement(card);

                        ScrollingMenu settingsMenu =
                            new(
                                "Please choose field to update:",
                                choices:
                                [
                                    "Change first name",
                                    "Change last name",
                                    "Change password",
                                    "Back"
                                ]
                            );
                        Window.AddElement(settingsMenu);
                        Window.ActivateElement(settingsMenu);

                        var settingsMenuResponse = settingsMenu.GetResponse();

                        switch (settingsMenuResponse!.Value)
                        {
                            case 0:
                                Flow.UpdateCustomerFirstName(Navigation.UserEmail);
                                card.UpdateLines(
                                    Component.GetCustomerCard(Navigation.UserEmail).Lines
                                );
                                break;

                            case 1:
                                Flow.UpdateCustomerLastName(Navigation.UserEmail);
                                card.UpdateLines(
                                    Component.GetCustomerCard(Navigation.UserEmail).Lines
                                );

                                break;

                            case 2:
                                Flow.UpdateCustomerPassword(Navigation.UserEmail);
                                card.UpdateLines(
                                    Component.GetCustomerCard(Navigation.UserEmail).Lines
                                );
                                break;

                            case 3:
                                Window.RemoveElement(settingsMenu);
                                updateLoop = false;
                                break;
                        }
                    }
                    break;

                case 3:
                    Window.RemoveElement(homeMenu, card);
                    Navigation.LogOut();
                    return;
            }
        }
    }
}
