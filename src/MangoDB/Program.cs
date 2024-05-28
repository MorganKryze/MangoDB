namespace MangoDB;

class Program
{
    static void Main()
    {
        Window.Open();

        Navigation.Setup();

        if (Flow.Authentication())
        {
            Navigation.Redirection();
        }

        RepositoryImplementation.CloseConnection();
        Window.Close();
    }
}
