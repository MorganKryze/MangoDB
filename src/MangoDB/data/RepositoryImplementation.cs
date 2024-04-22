namespace MangoDB;

public class RepositoryImplementation : IRepository
{
    private static string connectionString = string.Empty;

    public static void InitRepository()
    {
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: ["../database/.env"]));
        var variables = DotEnv.Read(
            options: new DotEnvOptions(envFilePaths: ["../database/.env"])
        );
        StringBuilder builder = new();
        builder.Append("Host=" + variables["DB_HOST"] + ":" + variables["DB_PORT"] + ";");
        builder.Append("Username=" + variables["DB_USER"] + ";");
        builder.Append("Password=" + variables["DB_PASSWORD"] + ";");
        builder.Append("Database=" + variables["DB_NAME"] + ";");
        connectionString = builder.ToString();
    }

    public static void CreateCustomer(
        string email,
        string first_name,
        string last_name,
        string password,
        int order_count,
        Profile loyalty_rank
    ) { }
}
