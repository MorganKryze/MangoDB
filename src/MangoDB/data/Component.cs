namespace MangoDB;

public class Component
{
    public static void WelcomeMessage()
    {
        Dialog welcomeDialog =
            new(
                [
                    "Welcome to the MangoDB smoothie shop",
                    "Please enter your ID and password to continue."
                ],
                rightOption: "OK"
            );
        Window.AddElement(welcomeDialog);
        Window.ActivateElement(welcomeDialog);
        Window.RemoveElement(welcomeDialog);
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
        Prompt usernamePrompt = new("Please enter a username:", defaultValue);
        Window.AddElement(usernamePrompt);

        Window.ActivateElement(usernamePrompt);
        var resp = usernamePrompt.GetResponse();
        Window.RemoveElement(usernamePrompt);

        return resp;
    }

    public static InteractionEventArgs<string>? GetEmail(string? defaultValue = null)
    {
        Prompt emailPrompt = new("Please enter an email:", defaultValue);
        Window.AddElement(emailPrompt);

        Window.ActivateElement(emailPrompt);
        var resp = emailPrompt.GetResponse();
        Window.RemoveElement(emailPrompt);

        return resp;
    }

    public static InteractionEventArgs<string>? GetFirstName(string? defaultValue = null)
    {
        Prompt first_namePrompt = new("Please enter a first name:", defaultValue);
        Window.AddElement(first_namePrompt);

        Window.ActivateElement(first_namePrompt);
        var resp = first_namePrompt.GetResponse();
        Window.RemoveElement(first_namePrompt);

        return resp;
    }

    public static InteractionEventArgs<string>? GetLastName(string? defaultValue = null)
    {
        Prompt last_namePrompt = new("Please enter a last name:", defaultValue);
        Window.AddElement(last_namePrompt);

        Window.ActivateElement(last_namePrompt);
        var resp = last_namePrompt.GetResponse();
        Window.RemoveElement(last_namePrompt);

        return resp;
    }

    public static InteractionEventArgs<string>? GetPassword(string? defaultValue = null)
    {
        Prompt passwordPrompt =
            new("Please enter a password:", defaultValue, style: PromptInputStyle.Secret);
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
}
