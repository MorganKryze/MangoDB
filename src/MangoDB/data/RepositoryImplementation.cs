namespace MangoDB;

public class RepositoryImplementation : IRepository
{
    private static NpgsqlConnection? conn;

    public static bool InitRepository()
    {
        try
        {
            var variables = DotEnv.Read(
                options: new DotEnvOptions(
                    envFilePaths: ["../database/.env", "../../database/.env"]
                )
            );
            StringBuilder builder = new();
            builder.Append("Host=" + variables["DB_HOST"] + ":" + variables["DB_PORT"] + ";");
            builder.Append("Username=" + variables["DB_USER"] + ";");
            builder.Append("Password=" + variables["DB_PASSWORD"] + ";");
            builder.Append("Database=" + variables["DB_NAME"] + ";");

            string connectionString = builder.ToString();

            conn = new NpgsqlConnection(connectionString);
            conn.Open();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
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
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "INSERT INTO customer (email, first_name, last_name, password, order_count, loyalty_rank) VALUES (@e, @f, @l, @p, @o, @r)";
        cmd.Parameters.AddWithValue("e", email);
        cmd.Parameters.AddWithValue("f", first_name);
        cmd.Parameters.AddWithValue("l", last_name);
        cmd.Parameters.AddWithValue("p", password);
        cmd.Parameters.AddWithValue("o", order_count);
        cmd.Parameters.AddWithValue("r", loyalty_rank);

        cmd.ExecuteNonQuery();
    }

    public static List<List<string>> GetCustomers(int limit)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT * FROM customer LIMIT @l";
        cmd.Parameters.AddWithValue("l", limit);

        using var reader = cmd.ExecuteReader();
        List<List<string>> customers = new();
        while (reader.Read())
        {
            List<string> customer = new();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (i == 3)
                {
                    customer.Add("********");
                    continue;
                }
                string value = reader[i]?.ToString() ?? string.Empty;
                customer.Add(value);
            }
            customers.Add(customer);
        }
        return customers;
    }

    public static bool CheckUser(string email, string table)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT * FROM " + table + " WHERE email = @e";
        cmd.Parameters.AddWithValue("e", email);

        using var reader = cmd.ExecuteReader();
        return reader.Read();
    }

    public static bool CheckPassword(string email, string password, string table)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT * FROM " + table + " WHERE email = @e AND password = @p";
        cmd.Parameters.AddWithValue("e", email);
        cmd.Parameters.AddWithValue("p", password);

        using var reader = cmd.ExecuteReader();
        return reader.Read();
    }
}
