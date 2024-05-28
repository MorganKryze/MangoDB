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
                    // Todo: Activate the card

                    // Todo: Add ScrollingMenu for settings (add back)

                    // TODO: replace scrolling with field (think of errors)

                    // TODO: Apply changes and reload card and display menu again

                    break;

                case 3:
                    Window.RemoveElement(homeMenu, card);
                    Navigation.LogOut();
                    return;
            }
        }
    }
}
