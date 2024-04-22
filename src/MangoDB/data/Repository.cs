namespace MangoDB;

interface IRepository
{
    public void CreateCustomer(
        string email,
        string first_name,
        string last_name,
        string password,
        int order_count,
        Profile loyalty_rank
    );
}
