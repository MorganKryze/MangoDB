namespace MangoDB;

public class RepositoryImplementation : IRepository
{
    private static Npgsql.NpgsqlConnection? conn;

    public static void InitRepository()
    {
        var variables = DotEnv.Read(
            options: new DotEnvOptions(envFilePaths: ["../database/.env", "../../database/.env"])
        );
        StringBuilder builder = new();
        builder.Append("Host=" + variables["DB_HOST"] + ":" + variables["DB_PORT"] + ";");
        builder.Append("Username=" + variables["DB_USER"] + ";");
        builder.Append("Password=" + variables["DB_PASSWORD"] + ";");
        builder.Append("Database=" + variables["DB_NAME"] + ";");

        string connectionString = builder.ToString();

        conn = new NpgsqlConnection(connectionString);
        conn.Open();
    }

    public static void CloseConnection()
    {
        conn!.Close();
    }

    public static void CreateCustomer(
        string email,
        string first_name,
        string last_name,
        string password,
        int order_count,
        string loyalty_rank
    )
    {
        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = conn;

            cmd.CommandText =
                "INSERT INTO customer (email, first_name, last_name, password, order_count, loyalty_rank) VALUES (@e, @f, @l, @p, @o, CAST(@r AS loyalty_rank))";
            cmd.Parameters.AddWithValue("e", email);
            cmd.Parameters.AddWithValue("f", first_name);
            cmd.Parameters.AddWithValue("l", last_name);
            cmd.Parameters.AddWithValue("p", password);
            cmd.Parameters.AddWithValue("o", order_count);
            cmd.Parameters.AddWithValue("r", loyalty_rank);

            cmd.ExecuteNonQuery();
        }
    }
}
