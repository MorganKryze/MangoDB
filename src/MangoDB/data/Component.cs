namespace MangoDB;

public class Component
{
    public static void WelcomeMessage()
    {
        Dialog welcomeDialog =
            new(
                ["Welcome to the MangoDB smoothie shop!", "Press enter to continue."],
                rightOption: "OK"
            );
        Window.AddElement(welcomeDialog);
        Window.ActivateElement(welcomeDialog);

        var response = welcomeDialog.GetResponse();
        if (response!.Status == Status.Escaped)
        {
            ConfirmExit();
        }
        Window.RemoveElement(welcomeDialog);
    }

    public static DialogOption ChooseSignOption()
    {
        Dialog signDialog = new(["Do you want to sign in or sign up?"], "Sign up", "Sign in");
        Window.AddElement(signDialog);
        Window.ActivateElement(signDialog);
        var signResponse = signDialog.GetResponse();
        Window.RemoveElement(signDialog);
        return signResponse!.Value;
    }

    public static InteractionEventArgs<int>? GetProfile()
    {
        string[] options = { "Mango Manager", "Mango Chef", "Customer", "Supplier" };
        ScrollingMenu profileSelector = new("Please select a profile:", choices: options);
        Window.AddElement(profileSelector);

        Window.ActivateElement(profileSelector);
        var resp = profileSelector.GetResponse();
        Window.RemoveElement(profileSelector);

        return resp;
    }

    public static InteractionEventArgs<string>? GetUsername(string? defaultValue = null)
    {
        Prompt usernamePrompt = new("Please enter a username:", defaultValue, maxInputLength: 20);
        Window.AddElement(usernamePrompt);

        Window.ActivateElement(usernamePrompt);
        var resp = usernamePrompt.GetResponse();
        Window.RemoveElement(usernamePrompt);

        return resp;
    }

    public static InteractionEventArgs<string>? GetEmail(string? defaultValue = null)
    {
        Prompt emailPrompt = new("Please enter an email:", defaultValue, maxInputLength: 40);
        Window.AddElement(emailPrompt);

        Window.ActivateElement(emailPrompt);
        var resp = emailPrompt.GetResponse();
        Window.RemoveElement(emailPrompt);

        return resp;
    }

    public static InteractionEventArgs<string>? GetFirstName(string? defaultValue = null)
    {
        Prompt first_namePrompt =
            new("Please enter a first name:", defaultValue, maxInputLength: 20);
        Window.AddElement(first_namePrompt);

        Window.ActivateElement(first_namePrompt);
        var resp = first_namePrompt.GetResponse();
        Window.RemoveElement(first_namePrompt);

        return resp;
    }

    public static InteractionEventArgs<string>? GetLastName(string? defaultValue = null)
    {
        Prompt last_namePrompt = new("Please enter a last name:", defaultValue, maxInputLength: 20);
        Window.AddElement(last_namePrompt);

        Window.ActivateElement(last_namePrompt);
        var resp = last_namePrompt.GetResponse();
        Window.RemoveElement(last_namePrompt);

        return resp;
    }

    public static InteractionEventArgs<string>? GetPassword(string? defaultValue = null)
    {
        Prompt passwordPrompt =
            new(
                "Please enter a password:",
                defaultValue,
                style: PromptInputStyle.Secret,
                maxInputLength: 30
            );
        Window.AddElement(passwordPrompt);

        Window.ActivateElement(passwordPrompt);
        var resp = passwordPrompt.GetResponse();
        Window.RemoveElement(passwordPrompt);

        return resp;
    }

    public static InteractionEventArgs<int>? GetOrderCount(int defaultValue = 0)
    {
        IntSelector order_count = new("Please select the order count:", 0, 30, defaultValue, 1);
        Window.AddElement(order_count);

        Window.ActivateElement(order_count);
        var resp = order_count.GetResponse();
        Window.RemoveElement(order_count);

        return resp;
    }

    public static EmbedText GetCustomerCard(string email)
    {
        var info = RepositoryImplementation.GetCustomerInfo(email);
        var orderCount = RepositoryImplementation.GetCustomerOrdersCount(email);
        return new EmbedText(
            [
                "Personal information:      ",
                "--------------------",
                "",
                $"First name: {info[0]}",
                $"Last name: {info[1]}",
                "",
                $"Loyalty rank: {info[2]}",
                $"Orders count: {orderCount}"
            ],
            placement: Placement.TopRight
        );
    }

    public static void ConfirmExit()
    {
        Dialog exitDialog = new(["Are you sure you want to exit?"], "No", "Yes");
        Window.AddElement(exitDialog);
        Window.ActivateElement(exitDialog);
        var resp = exitDialog.GetResponse();
        Window.RemoveElement(exitDialog);

        if (resp!.Value == DialogOption.Right)
        {
            RepositoryImplementation.CloseConnection();
            Window.Close();
        }
    }

    public static List<Recipe> GetRecipes()
    {
        var (names, prices) = RepositoryImplementation.GetRecipesNamesAndPrices();
        var ingredients = RepositoryImplementation.GetRecipesIngredients();
        var tools = RepositoryImplementation.GetRecipesTools();
        var steps = RepositoryImplementation.GetRecipesSteps();
        var allergens = RepositoryImplementation.GetRecipesAllergens();
        var calories = RepositoryImplementation.GetRecipesCalories();

        List<Recipe> recipes = [];
        for (int i = 0; i < names.Count; i++)
        {
            if (!allergens.TryGetValue(names[i], out List<string>? allergenList))
            {
                allergenList = ["None"];
            }

            recipes.Add(
                new Recipe(
                    names[i],
                    prices[i],
                    ingredients[names[i]],
                    tools[names[i]],
                    steps[names[i]],
                    allergenList,
                    calories[names[i]]
                )
            );
        }

        return recipes;
    }

    public static InteractionEventArgs<int>? SelectRecipe(List<Recipe> recipes)
    {
        List<string> headers = new() { "Name", "Price", "Calories", "Ingredients", "Allergens" };
        List<List<string>> options = [];
        for (int i = 0; i < recipes.Count; i++)
        {
            options.Add(
                [
                    recipes[i].Name,
                    recipes[i].Price.ToString(),
                    recipes[i].Calories.ToString(),
                    string.Join(", ", recipes[i].Ingredients.Select(ing => ing.Ingredient)),
                    string.Join(", ", recipes[i].Allergens)
                ]
            );
        }

        TableSelector recipeSelector = new("Please select a recipe:", headers, options);
        Window.AddElement(recipeSelector);

        Window.ActivateElement(recipeSelector);
        var resp = recipeSelector.GetResponse();
        Window.RemoveElement(recipeSelector);

        return resp;
    }

    public static InteractionEventArgs<int>? SelectNumber(int recipeSelected, List<Recipe> recipes)
    {
        EmbedText recipeInfo =
            new(
                [
                    "Recipe information:",
                    "",
                    $"Name: {recipes[recipeSelected].Name}",
                    $"Price: {recipes[recipeSelected].Price}",
                    $"Calories: {recipes[recipeSelected].Calories}"
                ],
                placement: Placement.TopRight
            );

        EmbedText info =
            new(
                [
                    "Info note:",
                    "",
                    "The quantity should be between 1 and 10.",
                    "If you want to order more, please contact the manager."
                ],
                placement: Placement.TopLeft
            );

        Window.AddElement(recipeInfo, info);
        Window.Render(recipeInfo, info);

        IntSelector quantitySelector =
            new("Select the quantity of the recipe:", min: 1, max: 10, start: 1, step: 1);
        Window.AddElement(quantitySelector);

        Window.ActivateElement(quantitySelector);
        var resp = quantitySelector.GetResponse();

        Window.DeactivateElement(recipeInfo);
        Window.DeactivateElement(info);
        Window.RemoveElement(recipeInfo, info, quantitySelector);
        Window.Clear();
        Window.Render();

        return resp;
    }

    public static DialogOption ConfirmCancelOrder()
    {
        Dialog cancelDialog = new(["Are you sure you want to cancel the order?"], "Yes", "No");
        Window.AddElement(cancelDialog);
        Window.ActivateElement(cancelDialog);
        var resp = cancelDialog.GetResponse();
        Window.RemoveElement(cancelDialog);

        return resp!.Value;
    }

    public static float GetTotalPrice(Dictionary<string, int> order, List<Recipe> recipes)
    {
        float totalPrice = 0;
        foreach (var (recipeName, quantity) in order)
        {
            var recipe = recipes.Find(r => r.Name == recipeName);
            if (recipe != null)
            {
                totalPrice += recipe.Price * quantity;
            }
        }

        return MathF.Round(totalPrice, 1);
    }

    public static EmbedText GetSupplierCard(string email)
    {
        var info = RepositoryImplementation.GetSupplierInfo(email);
        return new EmbedText(
            [
                "Personal information:      ",
                "--------------------",
                "",
                $"Company name: {info[0]}",
                $"Address: {info[1]}",
                "",
                $"Price category: {info[2]}"
            ],
            placement: Placement.TopRight
        );
    }

    public static void SeeIngredients(string email)
    {
        var ingredients = RepositoryImplementation.GetIngredients(email);
        List<string> headers = ["Name", "Price", "Calories", "Stock", "Allergen", "Origin"];
        List<List<string>> options = [];
        foreach (var ingredient in ingredients)
        {
            options.Add(
                [
                    ingredient.Name,
                    ingredient.Price.ToString(),
                    ingredient.Calories.ToString(),
                    ingredient.Stock.ToString(),
                    ingredient.Allergen,
                    ingredient.Origin
                ]
            );
        }

        TableView ingredientsViewer = new("Ingredients list:", headers, options);
        Window.AddElement(ingredientsViewer);
        Window.Render(ingredientsViewer);

        Dialog exitDialog = new(["Press enter to exit"]);
        Window.AddElement(exitDialog);
        Window.ActivateElement(exitDialog);

        Window.DeactivateElement(ingredientsViewer);
        Window.RemoveElement(ingredientsViewer, exitDialog);
    }

    public static void SeeAllIngredients()
    {
        var ingredients = RepositoryImplementation.GetAllIngredients();
        List<string> headers = ["Name", "Price", "Calories", "Stock", "Allergen", "Origin"];
        List<List<string>> options = [];
        foreach (var ingredient in ingredients)
        {
            options.Add(
                [
                    ingredient.Name,
                    ingredient.Price.ToString(),
                    ingredient.Calories.ToString(),
                    ingredient.Stock.ToString(),
                    ingredient.Allergen,
                    ingredient.Origin
                ]
            );
        }

        TableView ingredientsViewer = new("Ingredients list:", headers, options);
        Window.AddElement(ingredientsViewer);
        Window.Render(ingredientsViewer);

        Dialog exitDialog = new(["Press enter to exit"]);
        Window.AddElement(exitDialog);
        Window.ActivateElement(exitDialog);

        Window.DeactivateElement(ingredientsViewer);
        Window.RemoveElement(ingredientsViewer, exitDialog);
    }

    public static EmbedText GetMangoChefCard(string email)
    {
        var info = RepositoryImplementation.GetMangoChefInfo(email);
        return new EmbedText(
            [
                "Personal information:      ",
                "--------------------",
                "",
                $"First name: {info[0]}",
                $"Last name: {info[1]}",
                "",
                $"Salary: {info[2]}",
                $"Working hours: {info[3]}"
            ],
            placement: Placement.TopRight
        );
    }

    public static void SeeAllTools()
    {
        var tools = RepositoryImplementation.GetAllTools();
        List<string> headers = ["Name", "Price", "Stock"];
        List<List<string>> options = [];
        foreach (var tool in tools)
        {
            options.Add([tool.Name, tool.Price.ToString(), tool.Stock.ToString()]);
        }

        TableView toolsViewer = new("Tools list:", headers, options);
        Window.AddElement(toolsViewer);
        Window.Render(toolsViewer);

        Dialog exitDialog = new(["Press enter to exit"]);
        Window.AddElement(exitDialog);
        Window.ActivateElement(exitDialog);

        Window.DeactivateElement(toolsViewer);
        Window.RemoveElement(toolsViewer, exitDialog);
    }

    public static void ViewChefs()
    {
        IntSelector chefSelector = new("Please select a chef:", 1, 10, 5, 1);
        Window.AddElement(chefSelector);

        Window.ActivateElement(chefSelector);
        var resp = chefSelector.GetResponse();
        Window.RemoveElement(chefSelector);

        if (resp!.Status is Status.Escaped)
        {
            return;
        }

        var chefs = RepositoryImplementation.GetChefs(resp.Value);
        List<string> headers = ["Email", "First name", "Last name", "Working hours", "Salary"];
        List<List<string>> options = [];
        foreach (var chef in chefs)
        {
            options.Add([chef[0], chef[1], chef[2], chef[3], chef[4]]);
        }

        TableView chefsViewer = new("Chefs list:", headers, options);
        Window.AddElement(chefsViewer);
        Window.Render(chefsViewer);

        Dialog exitDialog = new(["Press enter to exit"]);
        Window.AddElement(exitDialog);
        Window.ActivateElement(exitDialog);

        Window.DeactivateElement(chefsViewer);
        Window.RemoveElement(chefsViewer, exitDialog);
    }

    public static void ViewSuppliers()
    {
        IntSelector supplierSelector = new("Please select a supplier:", 1, 10, 5, 1);
        Window.AddElement(supplierSelector);

        Window.ActivateElement(supplierSelector);
        var resp = supplierSelector.GetResponse();
        Window.RemoveElement(supplierSelector);

        if (resp!.Status is Status.Escaped)
        {
            return;
        }

        var suppliers = RepositoryImplementation.GetSuppliers(resp.Value);
        List<string> headers = ["Email", "Company name", "Address", "Price category"];
        List<List<string>> options = [];
        foreach (var supplier in suppliers)
        {
            options.Add([supplier[0], supplier[1], supplier[2], supplier[3]]);
        }

        TableView suppliersViewer = new("Suppliers list:", headers, options);
        Window.AddElement(suppliersViewer);
        Window.Render(suppliersViewer);

        Dialog exitDialog = new(["Press enter to exit"]);
        Window.AddElement(exitDialog);
        Window.ActivateElement(exitDialog);

        Window.DeactivateElement(suppliersViewer);
        Window.RemoveElement(suppliersViewer, exitDialog);
    }

    public static InteractionEventArgs<string>? GetSupplierName(string? defaultValue = null)
    {
        Prompt supplierNamePrompt =
            new("Please enter the supplier name:", defaultValue, maxInputLength: 30);
        Window.AddElement(supplierNamePrompt);

        Window.ActivateElement(supplierNamePrompt);
        var resp = supplierNamePrompt.GetResponse();
        Window.RemoveElement(supplierNamePrompt);

        return resp;
    }

    public static InteractionEventArgs<string>? GetSupplierAddress(string? defaultValue = null)
    {
        Prompt supplierAddressPrompt =
            new("Please enter the supplier address:", defaultValue, maxInputLength: 50);
        Window.AddElement(supplierAddressPrompt);

        Window.ActivateElement(supplierAddressPrompt);
        var resp = supplierAddressPrompt.GetResponse();
        Window.RemoveElement(supplierAddressPrompt);

        return resp;
    }

    public static InteractionEventArgs<int>? GetSupplierPriceCategory()
    {
        string[] options = { "Low", "Medium", "High" };
        ScrollingMenu priceCategorySelector =
            new("Please select the price category:", choices: options);
        Window.AddElement(priceCategorySelector);

        Window.ActivateElement(priceCategorySelector);
        var resp = priceCategorySelector.GetResponse();
        Window.RemoveElement(priceCategorySelector);

        return resp;
    }

    public static void SeeRecordsCount()
    {
        var (counts, total) = RepositoryImplementation.GetRecordsCount();
        List<string> headers = ["Table", "Count"];
        List<List<string>> options = [];
        foreach (var (table, count) in counts)
        {
            options.Add([table, count.ToString()]);
        }

        TableView countsViewer = new($"Records count, total: {total}", headers, options);
        Window.AddElement(countsViewer);
        Window.Render(countsViewer);

        Dialog exitDialog = new(["Press enter to exit"]);
        Window.AddElement(exitDialog);
        Window.ActivateElement(exitDialog);

        Window.DeactivateElement(countsViewer);
        Window.RemoveElement(countsViewer, exitDialog);
    }
}
