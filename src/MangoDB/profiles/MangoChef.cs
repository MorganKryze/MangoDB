namespace MangoDB;

public class MangoChef
{
    public static void HomePage()
    {
        EmbedText card = Component.GetMangoChefCard(Navigation.UserEmail);
        Window.AddElement(card);

        ScrollingMenu homeMenu =
            new(
                "Please choose an action:",
                choices:
                [
                    "Update order status",
                    "See Ingredients",
                    "See Tools",
                    "See recipes",
                    "Log out"
                ]
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
                    Flow.UpdateOrderStatus(Navigation.UserEmail);
                    break;

                case 1:
                    Component.SeeAllIngredients();
                    break;

                case 2:
                    Component.SeeAllTools();
                    break;

                case 3:
                    Flow.SeeRecipes();
                    break;

                case 4:
                    Window.RemoveElement(homeMenu, card);
                    Navigation.LogOut();
                    return;
            }
        }
    }
}
