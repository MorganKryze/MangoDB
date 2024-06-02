namespace MangoDB;

interface IRepository
{
    public static void CreateCustomer(
        string email,
        string first_name,
        string last_name,
        string password,
        string loyalty_rank
    )
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetCustomers(int limit)
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetChefs(int limit)
    {
        throw new NotImplementedException();
    }

    public static void AddChef(
        string email,
        string first_name,
        string last_name,
        string password,
        string working_hours,
        float salary
    )
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetSuppliers(int limit)
    {
        throw new NotImplementedException();
    }

    public static List<string> GetCustomerInfo(string email)
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetCustomerOrders(string email, int limit)
    {
        throw new NotImplementedException();
    }

    public static int GetCustomerOrdersCount(string email)
    {
        throw new NotImplementedException();
    }

    public static void UpdateCustomer(string email, string field, string value)
    {
        throw new NotImplementedException();
    }

    public static bool CheckUser(string email, string table)
    {
        throw new NotImplementedException();
    }

    public static bool CheckPassword(string email, string password, string table)
    {
        throw new NotImplementedException();
    }

    public static (List<string>, List<int>) GetRecipesNamesAndPrices()
    {
        throw new NotImplementedException();
    }

    public static Dictionary<string, List<IngredientQuantity>> GetRecipesIngredients()
    {
        throw new NotImplementedException();
    }

    public static Dictionary<string, List<string>> GetRecipesTools()
    {
        throw new NotImplementedException();
    }

    public static Dictionary<string, List<string>> GetRecipesSteps()
    {
        throw new NotImplementedException();
    }

    public static Dictionary<string, List<string>> GetRecipesAllergens()
    {
        throw new NotImplementedException();
    }

    public static Dictionary<string, int> GetRecipesCalories()
    {
        throw new NotImplementedException();
    }

    public static bool ConfirmOrder(Dictionary<string, int> order, float price, string email)
    {
        throw new NotImplementedException();
    }

    public static string GetRandomChefEmail()
    {
        throw new NotImplementedException();
    }

    public static List<string> GetSupplierInfo(string email)
    {
        throw new NotImplementedException();
    }

    public static List<Ingredient> GetIngredients(string email)
    {
        throw new NotImplementedException();
    }

    public static bool AddIngredient(Ingredient ingredient, string email)
    {
        throw new NotImplementedException();
    }

    public static bool UpdateIngredientPrice(string email, string ingredient_name, float price)
    {
        throw new NotImplementedException();
    }

    public static List<Ingredient> GetAllIngredients()
    {
        throw new NotImplementedException();
    }

    public static List<string> GetMangoChefInfo(string email)
    {
        throw new NotImplementedException();
    }

    public static List<Tool> GetAllTools()
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetChefOrders(string email)
    {
        throw new NotImplementedException();
    }

    public static void UpdateOrderStatus(string email, string order_time, string newStatus)
    {
        throw new NotImplementedException();
    }

    public static (Dictionary<string, int>, int) GetRecordsCount()
    {
        throw new NotImplementedException();
    }
}
