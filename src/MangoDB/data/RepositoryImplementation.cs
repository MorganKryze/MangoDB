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
            var outOfStockIngredients = new List<string>();
            foreach (var (recipe, quantity) in order)
            {
                cmd.CommandText =
                    $"SELECT ingredient_name, quantity FROM recipe_ingredient WHERE recipe_name = '{recipe}'";
                using var reader = cmd.ExecuteReader();

                var ingredients = new List<(string name, int quantity)>();
                while (reader.Read())
                {
                    var ingredientName = reader.GetString(0);
                    var ingredientQuantity = reader.GetInt32(1);

                    ingredients.Add((ingredientName, ingredientQuantity));
                }
                reader.Close();

                foreach (var (ingredientName, ingredientQuantity) in ingredients)
                {
                    cmd.CommandText =
                        $"SELECT in_stock FROM ingredient WHERE name = '{ingredientName}'";
                    var stock = (int)(cmd.ExecuteScalar() ?? 0);

                    if (stock < ingredientQuantity * quantity)
                    {
                        outOfStockIngredients.Add(ingredientName);
                    }
                }
            }
            if (outOfStockIngredients.Count > 0)
            {
                throw new OutOfStockException();
            }

            var orderTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var orderTime = DateTime.ParseExact(
                orderTimeString,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture
            );


            cmd.CommandText =
                "INSERT INTO \"order\" (time, customer_email, mango_chef_email, price, status) VALUES (CAST(@t AS TIMESTAMP), @e, @chef, @p, CAST('Pending' AS order_status))";
            cmd.Parameters.AddWithValue("e", Navigation.UserEmail);
            cmd.Parameters.AddWithValue("chef", email);
            cmd.Parameters.AddWithValue("p", price);
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

            foreach (var (recipe, quantity) in order)
            {
                cmd.CommandText =
                    $"SELECT ingredient_name, quantity FROM recipe_ingredient WHERE recipe_name = '{recipe}'";
                using var reader = cmd.ExecuteReader();

                var ingredients = new List<(string name, int quantity)>();
                while (reader.Read())
                {
                    var ingredientName = reader.GetString(0);
                    var ingredientQuantity = reader.GetInt32(1);

                    ingredients.Add((ingredientName, ingredientQuantity));
                }
                reader.Close();

                foreach (var (ingredientName, ingredientQuantity) in ingredients)
                {
                    cmd.CommandText =
                        $"UPDATE ingredient SET in_stock = in_stock - @q WHERE name = '@r'";
                    cmd.Parameters.AddWithValue("q", ingredientQuantity * quantity);
                    cmd.Parameters.AddWithValue("r", ingredientName);
                    cmd.ExecuteNonQuery();
                }
            }

            transaction?.Commit();

            return true;
        }
        catch
        {
            transaction?.Rollback();

            return false;
        }
    }

    public static List<string> GetSupplierInfo(string email)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT name, address, price_category FROM supplier_company WHERE email = @e";
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

    public static List<Ingredient> GetIngredients(string email)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT name, price, calories, in_stock, allergen, country FROM ingredient WHERE supplier_email = @e";
        cmd.Parameters.AddWithValue("e", email);

        using var reader = cmd.ExecuteReader();
        List<Ingredient> ingredients = [];
        while (reader.Read())
        {
            string name = reader.GetString(0);
            float price = reader.GetFloat(1);
            int calories = reader.GetInt32(2);
            int stock = reader.GetInt32(3);
            string allergen = reader.GetString(4);
            string origin = reader.GetString(5);

            Ingredient ingredient = new(name, price, calories, stock, allergen, origin);
            ingredients.Add(ingredient);
        }
        return ingredients;
    }

    public static bool AddIngredient(Ingredient ingredient, string email)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "INSERT INTO ingredient (name, price, calories, in_stock, allergen, country, supplier_email) VALUES (@n, @p, @c, @s, @a, @o, @e)";
        cmd.Parameters.AddWithValue("n", ingredient.Name);
        cmd.Parameters.AddWithValue("p", ingredient.Price);
        cmd.Parameters.AddWithValue("c", ingredient.Calories);
        cmd.Parameters.AddWithValue("s", ingredient.Stock);
        cmd.Parameters.AddWithValue("a", ingredient.Allergen);
        cmd.Parameters.AddWithValue("o", ingredient.Origin);
        cmd.Parameters.AddWithValue("e", email);

        try
        {
            cmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool UpdateIngredientPrice(string email, string ingredient_name, float price)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "UPDATE ingredient SET price = @p WHERE supplier_email = @e AND name = @n";
        cmd.Parameters.AddWithValue("p", MathF.Round(price, 1));
        cmd.Parameters.AddWithValue("e", email);
        cmd.Parameters.AddWithValue("n", ingredient_name);

        try
        {
            cmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static List<Ingredient> GetAllIngredients()
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT name, price, calories, in_stock, allergen, country FROM ingredient";

        using var reader = cmd.ExecuteReader();
        List<Ingredient> ingredients = [];
        while (reader.Read())
        {
            string name = reader.GetString(0);
            float price = reader.GetFloat(1);
            int calories = reader.GetInt32(2);
            int stock = reader.GetInt32(3);
            string allergen = reader.GetString(4);
            string origin = reader.GetString(5);

            Ingredient ingredient = new(name, price, calories, stock, allergen, origin);
            ingredients.Add(ingredient);
        }
        return ingredients;
    }

    public static List<string> GetMangoChefInfo(string email)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT first_name, last_name, salary, working_hours FROM mango_chef WHERE email = @e";
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

    public static List<Tool> GetAllTools()
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText = "SELECT name, price, in_stock FROM tool";

        using var reader = cmd.ExecuteReader();
        List<Tool> tools = [];
        while (reader.Read())
        {
            string name = reader.GetString(0);
            float price = reader.GetFloat(1);
            int stock = reader.GetInt32(2);

            Tool tool = new(name, price, stock);
            tools.Add(tool);
        }
        return tools;
    }

    public static List<List<string>> GetChefOrders(string email)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        cmd.CommandText =
            "SELECT time, price, status FROM \"order\" WHERE mango_chef_email = @e AND status = 'Pending' OR status = 'In Progress'";
        cmd.Parameters.AddWithValue("e", email);

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

    public static void UpdateOrderStatus(string order_time, string newStatus)
    {
        using var cmd = new NpgsqlCommand();
        cmd.Connection = conn;

        var orderDateTime = DateTime.ParseExact(
            order_time,
            "M/d/yyyy h:mm:ss tt",
            CultureInfo.InvariantCulture
        );

        var orderTimestamp = orderDateTime.ToString(
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture
        );

        cmd.CommandText =
            "UPDATE \"order\" SET status = CAST(@s AS \"order_status\") WHERE time = CAST(@t AS TIMESTAMP)";
        cmd.Parameters.AddWithValue("s", newStatus);
        cmd.Parameters.AddWithValue("t", orderTimestamp);
        Core.WriteDebugMessage(lines: cmd.ExecuteNonQuery().ToString());
    }
}
