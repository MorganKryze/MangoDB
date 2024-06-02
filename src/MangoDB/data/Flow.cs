namespace MangoDB;

public class Flow
{
    private static readonly string[] tables =
    [
        "mango_manager",
        "mango_chef",
        "customer",
        "supplier_company",
    ];

    public static void Sign()
    {
        SignOption:
        var response = Component.ChooseSignOption();
        if (response == DialogOption.None)
        {
            Component.ConfirmExit();
            goto SignOption;
        }
        else if (response == DialogOption.Left)
        {
            CreateCustomer();
            goto SignOption;
        }
    }

    public static bool Authentication()
    {
        Prompt id = new("Please enter your email:", maxInputLength: 40);
        Dialog wrongId =
            new(
                ["Wrong ID", string.Empty, "The ID you entered has no match in the database."],
                "Quit",
                "Try again"
            );
        Window.AddElement(id, wrongId);

        string idValue = "";
        string table = "";
        int counter = 0;
        bool loop = true;

        while (loop)
        {
            Window.ActivateElement(id);
            var idResponse = id.GetResponse();
            for (int i = 0; i < tables.Length; i++)
            {
                if (RepositoryImplementation.CheckUser(idResponse!.Value, tables[i]))
                {
                    idValue = idResponse!.Value;
                    table = tables[i];
                    loop = false;
                    break;
                }
                if (i == tables.Length - 1)
                {
                    Window.ActivateElement(wrongId);
                    var wrongIdResponse = wrongId.GetResponse();
                    if (wrongIdResponse!.Value == DialogOption.Left)
                    {
                        Window.RemoveElement(id, wrongId);
                        return false;
                    }
                    continue;
                }
            }
        }

        Prompt password =
            new("Please enter your password:", style: PromptInputStyle.Secret, maxInputLength: 30);
        Dialog wrongPassword =
            new(
                ["Wrong Password", string.Empty, "The password you entered is incorrect."],
                "Quit",
                "Try again"
            );
        Dialog failedAuth =
            new(
                [
                    "Access Denied",
                    string.Empty,
                    "The authentication process failed (3 attempts).",
                    "You may try again later."
                ],
                rightOption: "OK"
            );
        Window.AddElement(password, wrongPassword, failedAuth);

        string passwordValue;
        while (true)
        {
            Window.ActivateElement(password);
            var passwordResponse = password.GetResponse();
            string hashedPassword = Tool.Hash(passwordResponse!.Value);
            if (!RepositoryImplementation.CheckPassword(idValue, hashedPassword, table))
            {
                if (counter == 2)
                {
                    Window.ActivateElement(failedAuth);
                    Window.RemoveElement(id, password, wrongId, wrongPassword, failedAuth);
                    return false;
                }
                Window.ActivateElement(wrongPassword);
                var wrongPasswordResponse = wrongPassword.GetResponse();
                if (wrongPasswordResponse!.Value == DialogOption.Left)
                {
                    return false;
                }
                counter++;
                continue;
            }
            passwordValue = passwordResponse.Value;
            break;
        }

        FakeLoadingBar profileLoadingBar = new("[ Loading profile... ]");
        Window.AddElement(profileLoadingBar);
        Window.ActivateElement(profileLoadingBar);

        Profile userProfile = table switch
        {
            "mango_manager" => Profile.MangoManager,
            "mango_chef" => Profile.MangoChef,
            "customer" => Profile.Customer,
            "supplier" => Profile.Supplier,
            _ => Profile.None
        };
        Navigation.LogIn(userProfile, idValue);

        Window.RemoveElement(id);
        Window.RemoveElement(password);
        Window.RemoveElement(wrongId);
        Window.RemoveElement(wrongPassword);
        Window.RemoveElement(profileLoadingBar);
        return true;
    }

    public static bool CreateCustomer()
    {
        string emailField = "",
            firstNameField = "",
            lastNameField = "",
            passwordField = "";
        string loyaltyRankField = "Classic";

        Email:
        var resp = Component.GetEmail(emailField);
        if (resp!.Status is Status.Escaped)
        {
            return false;
        }
        else
        {
            emailField = resp.Value;
        }

        FirstName:
        resp = Component.GetFirstName(firstNameField);
        if (resp!.Status is Status.Escaped)
        {
            goto Email;
        }
        else
        {
            firstNameField = resp.Value;
        }

        LastName:
        resp = Component.GetLastName(lastNameField);
        if (resp!.Status is Status.Escaped)
        {
            goto FirstName;
        }
        else
        {
            lastNameField = resp.Value;
        }

        resp = Component.GetPassword(passwordField);
        if (resp!.Status is Status.Escaped)
        {
            goto LastName;
        }
        else
        {
            passwordField = Tool.Hash(resp.Value);
        }

        try
        {
            RepositoryImplementation.CreateCustomer(
                emailField,
                firstNameField,
                lastNameField,
                passwordField,
                loyaltyRankField
            );
        }
        catch (Exception e)
        {
            Dialog errorDialog;
            if (e.Message.Substring(0, 5) == "23505")
            {
                errorDialog = new(
                    [
                        "Error",
                        "The email you entered is already in use.",
                        "Please try again with a different email."
                    ],
                    rightOption: "OK"
                );
            }
            else
            {
                errorDialog = new(
                    [
                        "Error",
                        "An error occurred when trying to create the user.",
                        "The operation will be cancelled."
                    ],
                    rightOption: "OK"
                );
            }
            Window.AddElement(errorDialog);
            Window.ActivateElement(errorDialog);

            Window.RemoveElement(errorDialog);
            return false;
        }

        return true;
    }

    public static bool ViewCustomersTable()
    {
        try
        {
            IntSelector limitSelector =
                new("Select the number of customers to view", min: 3, max: 15, start: 10, step: 1);
            Window.AddElement(limitSelector);

            Window.ActivateElement(limitSelector);
            var limitResp = limitSelector.GetResponse();
            if (limitResp!.Status is not Status.Selected)
            {
                Window.RemoveElement(limitSelector);
                return false;
            }

            var customers = RepositoryImplementation.GetCustomers(limitResp.Value);

            TableView customersTable =
                new(
                    title: $"Customers (first {limitResp.Value})",
                    headers: new List<string>()
                    {
                        "email",
                        "first_name",
                        "last_name",
                        "password",
                        "rank"
                    },
                    lines: customers
                );

            Window.AddElement(customersTable);
            Window.Render(customersTable);
            Window.Freeze();

            Window.DeactivateElement(customersTable);
            Window.RemoveElement(customersTable);
            return true;
        }
        catch (Exception)
        {
            Dialog errorDialog =
                new(
                    [
                        "Error",
                        "An error occurred when trying to fetch the customers.",
                        "The operation will be cancelled."
                    ],
                    rightOption: "OK"
                );
            Window.AddElement(errorDialog);
            Window.ActivateElement(errorDialog);

            Window.RemoveElement(errorDialog);
            return false;
        }
    }

    public static void MakeOrder()
    {
        Dictionary<string, int> cart = [];
        List<Recipe> recipes = Component.GetRecipes();

        SelectRecipe:

        var recipeSelected = Component.SelectRecipe(recipes);
        if (recipeSelected!.Status is not Status.Selected)
        {
            return;
        }

        var quantityResp = Component.SelectNumber(recipeSelected.Value, recipes);
        if (quantityResp!.Status is not Status.Selected)
        {
            goto SelectRecipe;
        }

        if (cart.ContainsKey(recipes[recipeSelected.Value].Name))
        {
            cart[recipes[recipeSelected.Value].Name] += quantityResp.Value;
            cart[recipes[recipeSelected.Value].Name] =
                cart[recipes[recipeSelected.Value].Name] > 10
                    ? 10
                    : cart[recipes[recipeSelected.Value].Name];
        }
        else
        {
            cart.Add(recipes[recipeSelected.Value].Name, quantityResp.Value);
        }

        Menu:

        TableView cartTable =
            new(
                title: "Cart",
                headers: ["Recipe", "Quantity"],
                lines: cart.Select(x => new List<string>() { x.Key, x.Value.ToString() }).ToList(),
                placement: Placement.TopRight
            );
        Window.AddElement(cartTable);
        Window.Render(cartTable);

        string[] options = ["Add another recipe", "Clear cart", "Confirm order", "Cancel"];
        ScrollingMenu menu = new("Please choose an option:", choices: options);
        Window.AddElement(menu);
        Window.ActivateElement(menu);

        var menuResp = menu.GetResponse();

        Window.DeactivateElement(cartTable);
        Window.RemoveElement(menu, cartTable);

        if (menuResp!.Status is not Status.Selected)
        {
            if (Component.ConfirmCancelOrder() is DialogOption.Right)
            {
                goto Menu;
            }
            else
            {
                return;
            }
        }

        switch (menuResp.Value)
        {
            case 0:
                goto SelectRecipe;
            case 1:
                cart.Clear();
                goto Menu;

            case 2:
                bool validation = RepositoryImplementation.ConfirmOrder(
                    cart,
                    Component.GetTotalPrice(cart, recipes),
                    RepositoryImplementation.GetRandomChefEmail()
                );

                FakeLoadingBar loadingBar = new("[ Placing order... ]");
                Window.AddElement(loadingBar);
                Window.ActivateElement(loadingBar);

                Window.RemoveElement(loadingBar);

                if (validation)
                {
                    Dialog orderConfirmation =
                        new(
                            [
                                "Order confirmation",
                                "",
                                "Your order has been successfully placed.",
                                $"The total price is {Component.GetTotalPrice(cart, recipes)}.",
                                "",
                                "Thank you for your order!",
                                "You will receive an email with the details."
                            ],
                            rightOption: "OK"
                        );
                    Window.AddElement(orderConfirmation);
                    Window.ActivateElement(orderConfirmation);

                    Window.RemoveElement(orderConfirmation);
                }
                else
                {
                    Dialog orderFailed =
                        new(
                            [
                                "Order failed",
                                "",
                                "An error occurred when trying to place the order.",
                                "Please try again later."
                            ],
                            rightOption: "OK"
                        );
                    Window.AddElement(orderFailed);
                    Window.ActivateElement(orderFailed);

                    Window.RemoveElement(orderFailed);
                }
                break;

            case 3:
                return;
        }
    }

    public static bool ShowHashed()
    {
        Prompt askText = new("Please enter the text to hash:");
        Window.AddElement(askText);
        Window.ActivateElement(askText);

        var textResponse = askText.GetResponse();
        if (textResponse!.Status is not Status.Selected)
        {
            Window.RemoveElement(askText);
            return false;
        }
        string hashed = Tool.Hash(textResponse!.Value);
        Dialog showHashed =
            new(["Hashed Text", hashed, "The hashed text is shown above."], rightOption: "OK");
        Window.AddElement(showHashed);
        Window.ActivateElement(showHashed);

        Window.RemoveElement(askText);
        Window.RemoveElement(showHashed);
        return true;
    }

    public static void UpdateCustomerFirstName(string email)
    {
        EmbedText info = new EmbedText(
            [
                "Info note:",
                "",
                "Please write down the new first name.",
                "Do not leave a blank field."
            ]
        );
        Window.AddElement(info);
        string previousFirstName = RepositoryImplementation.GetCustomerInfo(email)[0];
        Prompt newFirstNamePrompt =
            new("Type here the new first name:", previousFirstName, maxInputLength: 20);
        Window.AddElement(newFirstNamePrompt);

        while (true)
        {
            Window.Render(info);
            Window.ActivateElement(newFirstNamePrompt);
            var resp = newFirstNamePrompt.GetResponse();
            Window.DeactivateElement(info);
            Window.RemoveElement(info, newFirstNamePrompt);

            if (
                resp!.Status is Status.Selected
                && resp.Value != previousFirstName
                && resp.Value != string.Empty
            )
            {
                RepositoryImplementation.UpdateCustomer(email, "first_name", resp.Value);
                return;
            }
            else if (resp!.Status is Status.Escaped)
            {
                return;
            }
        }
    }

    public static void UpdateCustomerLastName(string email)
    {
        EmbedText info = new EmbedText(
            [
                "Info note:",
                "",
                "Please write down the new last name.",
                "Do not leave a blank field."
            ]
        );
        Window.AddElement(info);
        string previousLastName = RepositoryImplementation.GetCustomerInfo(email)[1];
        Prompt newLastNamePrompt =
            new("Type here the new last name:", previousLastName, maxInputLength: 20);
        Window.AddElement(newLastNamePrompt);

        while (true)
        {
            Window.Render(info);
            Window.ActivateElement(newLastNamePrompt);
            var resp = newLastNamePrompt.GetResponse();
            Window.DeactivateElement(info);
            Window.RemoveElement(info, newLastNamePrompt);

            if (
                resp!.Status is Status.Selected
                && resp.Value != previousLastName
                && resp.Value != string.Empty
            )
            {
                RepositoryImplementation.UpdateCustomer(email, "last_name", resp.Value);
                return;
            }
            else if (resp!.Status is Status.Escaped)
            {
                return;
            }
        }
    }

    public static void UpdateCustomerPassword(string email)
    {
        EmbedText info = new EmbedText(
            [
                "Info note:",
                "",
                "Please write down the new password.",
                "Do not leave a blank field.",
                "The password should be at least of 8 characters long."
            ]
        );
        Window.AddElement(info);
        Prompt newPasswordPrompt =
            new("Type here the new password:", style: PromptInputStyle.Secret, maxInputLength: 30);
        Window.AddElement(newPasswordPrompt);

        while (true)
        {
            Window.Render(info);
            Window.ActivateElement(newPasswordPrompt);
            var resp = newPasswordPrompt.GetResponse();
            Window.DeactivateElement(info);
            Window.RemoveElement(info, newPasswordPrompt);

            if (
                resp!.Status is Status.Selected
                && resp.Value != string.Empty
                && resp.Value.Length >= 8
            )
            {
                RepositoryImplementation.UpdateCustomer(email, "password", Tool.Hash(resp.Value));
                return;
            }
            else if (resp!.Status is Status.Escaped)
            {
                return;
            }
        }
    }

    public static void ViewCustomerOrders()
    {
        IntSelector limitSelector =
            new("Select the number of orders to view:", min: 3, max: 10, start: 5, step: 1);
        Window.AddElement(limitSelector);

        Window.ActivateElement(limitSelector);
        var limitResp = limitSelector.GetResponse();
        if (limitResp!.Status is not Status.Selected)
        {
            Window.RemoveElement(limitSelector);
            return;
        }

        var orders = RepositoryImplementation.GetCustomerOrders(
            Navigation.UserEmail,
            limitResp.Value
        );

        TableView ordersTable =
            new(
                title: $"Orders (first {limitResp.Value})",
                headers: ["time", "price", "status"],
                lines: orders
            );

        Window.AddElement(ordersTable);
        Window.Render(ordersTable);
        Dialog ordersDialog =
            new(
                [$"Your past {limitResp.Value} orders are shown above.", "Press enter to continue."]
            );
        Window.AddElement(ordersDialog);
        Window.ActivateElement(ordersDialog);

        Window.DeactivateElement(ordersTable);
        Window.RemoveElement(ordersTable, ordersDialog);
    }
}
