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
}
