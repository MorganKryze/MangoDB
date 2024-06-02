namespace MangoDB;

public class MangoManager
{
    public static void HomePage()
    {
        ScrollingMenu homeMenu =
            new(
                "Please choose an action:",
                choices:
                [
                    "View customers",
                    "Add a customer",
                    "View chefs",
                    "Add a chef",
                    "View suppliers",
                    "Add a supplier",
                    "See Ingredients",
                    "See Tools",
                    "See recipes",
                    "Hash text",
                    "Log out"
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
                    Component.ViewChefs();
                    break;

                case 3:

                    break;

                case 4:
                    Component.ViewSuppliers();
                    break;

                case 5:

                    break;

                case 6:
                    Component.SeeAllIngredients();
                    break;

                case 7:
                    Component.SeeAllTools();
                    break;

                case 8:
                    Flow.SeeRecipes();
                    break;

                case 9:
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

                case 10:
                    Navigation.LogOut();
                    return;
            }
        }
    }
}
