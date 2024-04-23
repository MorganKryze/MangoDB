namespace MangoDB;

interface IRepository
{
    public static void CreateCustomer(
        string email,
        string first_name,
        string last_name,
        string password,
        int order_count,
        string loyalty_rank
    )
    {
        throw new NotImplementedException();
    }

    public static List<List<string>> GetCustomers(int limit)
    {
        throw new NotImplementedException();
    }
}
