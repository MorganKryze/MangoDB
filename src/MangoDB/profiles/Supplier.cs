namespace MangoDB;

public class Supplier
{
    public static void HomePage()
    {
        EmbedText card = Component.GetSupplierCard(Navigation.UserEmail);
        Window.AddElement(card);

        ScrollingMenu homeMenu =
            new(
                "Please choose an action:",
                choices: ["See products", "Add product", "Update prices", "Log out"]
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
                    Component.SeeIngredients(Navigation.UserEmail);
                    break;

                case 1:
                    Flow.AddIngredient(Navigation.UserEmail);
                    break;

                case 2:
                    Flow.UpdateIngredientPrices(Navigation.UserEmail);
                    break;

                case 3:
                    Window.RemoveElement(homeMenu, card);
                    Navigation.LogOut();
                    return;
            }
        }
    }
}
