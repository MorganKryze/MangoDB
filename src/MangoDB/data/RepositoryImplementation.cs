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
        string loyalty_rank
    )
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "INSERT INTO customer (email, first_name, last_name, password, loyalty_rank) VALUES (@e, @f, @l, @p, CAST(@r AS loyalty_rank))";
        cmd.Parameters.AddWithValue("e", email);
        cmd.Parameters.AddWithValue("f", first_name);
        cmd.Parameters.AddWithValue("l", last_name);
        cmd.Parameters.AddWithValue("p", password);
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
        List<List<string>> customers = [];
        while (reader.Read())
        {
            List<string> customer = [];
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

    public static List<string> GetCustomerInfo(string email)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT first_name, last_name, loyalty_rank FROM customer WHERE email = @e";
        cmd.Parameters.AddWithValue("e", email);

        using var reader = cmd.ExecuteReader();
        reader.Read();
        List<string> customer = [];
        for (int i = 0; i < reader.FieldCount; i++)
        {
            string value = reader[i]?.ToString() ?? string.Empty;
            customer.Add(value);
        }
        return customer;
    }

    public static List<List<string>> GetCustomerOrders(string email, int limit)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT time, price, status FROM \"order\" WHERE customer_email = @e LIMIT @l";
        cmd.Parameters.AddWithValue("e", email);
        cmd.Parameters.AddWithValue("l", limit);

        using var reader = cmd.ExecuteReader();
        List<List<string>> orders = [];
        while (reader.Read())
        {
            List<string> order = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader[i]?.ToString() ?? string.Empty;
                order.Add(value);
            }
            orders.Add(order);
        }
        return orders;
    }

    public static int GetCustomerOrdersCount(string email)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT COUNT(*) FROM \"order\" WHERE customer_email = @e";
        cmd.Parameters.AddWithValue("e", email);

        using var reader = cmd.ExecuteReader();
        reader.Read();
        return reader.GetInt32(0);
    }

    public static void UpdateCustomer(string email, string field, string value)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "UPDATE customer SET " + field + " = @v WHERE email = @e";
        cmd.Parameters.AddWithValue("v", value);
        cmd.Parameters.AddWithValue("e", email);

        cmd.ExecuteNonQuery();
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

    public static (List<string>, List<float>) GetRecipesNamesAndPrices()
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT name, price FROM recipe";

        using var reader = cmd.ExecuteReader();
        List<string> names = [];
        List<float> prices = [];
        while (reader.Read())
        {
            names.Add(reader.GetString(0));
            prices.Add(reader.GetFloat(1));
        }
        return (names, prices);
    }

    public static Dictionary<string, List<IngredientQuantity>> GetRecipesIngredients()
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT recipe.name, ingredient.name, recipe_ingredient.quantity "
            + "FROM recipe "
            + "JOIN recipe_ingredient ON recipe.name = recipe_ingredient.recipe_name "
            + "JOIN ingredient ON recipe_ingredient.ingredient_name = ingredient.name";

        using var reader = cmd.ExecuteReader();
        Dictionary<string, List<IngredientQuantity>> recipeIngredients = [];

        while (reader.Read())
        {
            string recipeName = reader.GetString(0);
            string ingredientName = reader.GetString(1);
            int quantity = reader.GetInt32(2);

            if (!recipeIngredients.TryGetValue(recipeName, out List<IngredientQuantity>? value))
            {
                value = [];
                recipeIngredients.Add(recipeName, value);
            }

            IngredientQuantity ingredientQuantity = new(ingredientName, quantity);
            value.Add(ingredientQuantity);
        }

        return recipeIngredients;
    }

    public static Dictionary<string, List<string>> GetRecipesTools()
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT recipe.name, tool.name "
            + "FROM recipe "
            + "JOIN recipe_tool ON recipe.name = recipe_tool.recipe_name "
            + "JOIN tool ON recipe_tool.tool_name = tool.name";

        using var reader = cmd.ExecuteReader();
        Dictionary<string, List<string>> recipeTools = [];

        while (reader.Read())
        {
            string recipeName = reader.GetString(0);
            string toolName = reader.GetString(1);

            if (!recipeTools.TryGetValue(recipeName, out List<string>? value))
            {
                value = [];
                recipeTools.Add(recipeName, value);
            }

            value.Add(toolName);
        }

        return recipeTools;
    }

    public static Dictionary<string, List<string>> GetRecipesSteps()
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT recipe.name, step.description "
            + "FROM recipe "
            + "JOIN step ON recipe.name = step.recipe_name";

        using var reader = cmd.ExecuteReader();
        Dictionary<string, List<string>> recipeSteps = [];

        while (reader.Read())
        {
            string recipeName = reader.GetString(0);
            string stepDescription = reader.GetString(1);

            if (!recipeSteps.TryGetValue(recipeName, out List<string>? value))
            {
                value = [];
                recipeSteps.Add(recipeName, value);
            }

            value.Add(stepDescription);
        }

        return recipeSteps;
    }

    public static Dictionary<string, List<string>> GetRecipesAllergens()
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT recipe.name, ingredient.allergen "
            + "FROM recipe "
            + "JOIN recipe_ingredient ON recipe.name = recipe_ingredient.recipe_name "
            + "JOIN ingredient ON recipe_ingredient.ingredient_name = ingredient.name "
            + "WHERE ingredient.allergen IS NOT NULL AND ingredient.allergen <> 'None'";

        using var reader = cmd.ExecuteReader();
        Dictionary<string, List<string>> recipeAllergens = [];

        while (reader.Read())
        {
            string recipeName = reader.GetString(0);
            string allergenName = reader.GetString(1);

            if (!recipeAllergens.TryGetValue(recipeName, out List<string>? value))
            {
                value = [];
                recipeAllergens.Add(recipeName, value);
            }

            value.Add(allergenName);
        }

        return recipeAllergens;
    }

    public static string GetRandomChefEmail()
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT email FROM mango_chef ORDER BY RANDOM() LIMIT 1";

        using var reader = cmd.ExecuteReader();
        reader.Read();
        return reader.GetString(0);
    }

    public static Dictionary<string, int> GetRecipesCalories()
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT recipe.name, SUM(ingredient.calories * recipe_ingredient.quantity) "
            + "FROM recipe "
            + "JOIN recipe_ingredient ON recipe.name = recipe_ingredient.recipe_name "
            + "JOIN ingredient ON recipe_ingredient.ingredient_name = ingredient.name "
            + "GROUP BY recipe.name";

        using var reader = cmd.ExecuteReader();
        Dictionary<string, int> recipeCalories = new();

        while (reader.Read())
        {
            string recipeName = reader.GetString(0);
            int calories = reader.GetInt32(1);

            recipeCalories.Add(recipeName, calories);
        }

        return recipeCalories;
    }

    public static bool ConfirmOrder(Dictionary<string, int> order, float price, string email)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        var transaction = cmd.Connection?.BeginTransaction();

        try
        {
            var orderTime = DateTime.Now;

            cmd.CommandText =
                "INSERT INTO \"order\" (time, customer_email, mango_chef_email, price, status) VALUES (@t, @e, @chef, @p, CAST('Pending' AS order_status))";
            cmd.Parameters.AddWithValue("e", Navigation.UserEmail);
            cmd.Parameters.AddWithValue("chef", email);
            cmd.Parameters.AddWithValue("p", Math.Round(price, 1));
            cmd.Parameters.AddWithValue("t", orderTime);
            cmd.ExecuteNonQuery();

            var sb = new StringBuilder();
            sb.Append("INSERT INTO order_recipe (order_time, recipe_name, quantity) VALUES ");

            var i = 0;
            foreach (var (recipe, quantity) in order)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }
                sb.Append($"(@t{i}, @r{i}, @q{i})");

                cmd.Parameters.AddWithValue($"t{i}", orderTime);
                cmd.Parameters.AddWithValue($"r{i}", recipe);
                cmd.Parameters.AddWithValue($"q{i}", quantity);

                i++;
            }

            cmd.CommandText = sb.ToString();
            cmd.ExecuteNonQuery();

            transaction?.Commit();

            return true;
        }
        catch (Exception e)
        {
            Core.WriteDebugMessage(lines: e.Message);
            transaction?.Rollback();

            return false;
        }
    }
}
