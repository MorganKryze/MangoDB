namespace MangoDB;

public class RepositoryImplementation : IRepository
{
    public void CreateCustomer(
        string email,
        string first_name,
        string last_name,
        string password,
        int order_count,
        Profile loyalty_rank
    ) { }
}
