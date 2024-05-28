namespace MangoDB;

class Program
{
    static void Main()
    {
        Window.Open();

        Navigation.Setup();

        Component.WelcomeMessage();

        SignOptions:
        Flow.Sign();

        if (Flow.Authentication())
        {
            Navigation.Redirection();
        }
        goto SignOptions;
    }
}
