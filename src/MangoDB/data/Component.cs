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
}
