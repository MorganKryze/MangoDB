namespace MangoDB;

public class Flow
{
    public static bool CreateCustomer()
    {
        string emailField = "",
            firstNameField = "",
            lastNameField = "",
            passwordField = "";
        int orderCountField = 0;
        string loyaltyRankField = "Classic";

        Email:
        var resp = Component.GetEmail(emailField);
        if (resp!.Status is Status.Escaped)
        {
            return false;
        }
        else
        {
            emailField = resp.Value;
        }

        FirstName:
        resp = Component.GetFirstName(firstNameField);
        if (resp!.Status is Status.Escaped)
        {
            goto Email;
        }
        else
        {
            firstNameField = resp.Value;
        }

        LastName:
        resp = Component.GetLastName(lastNameField);
        if (resp!.Status is Status.Escaped)
        {
            goto FirstName;
        }
        else
        {
            lastNameField = resp.Value;
        }

        resp = Component.GetPassword(passwordField);
        if (resp!.Status is Status.Escaped)
        {
            goto LastName;
        }
        else
        {
            passwordField = Tool.Hash(resp.Value);
        }

        try
        {
            RepositoryImplementation.CreateCustomer(
                emailField,
                firstNameField,
                lastNameField,
                passwordField,
                orderCountField,
                loyaltyRankField
            );
        }
        catch (Exception)
        {
            Dialog errorDialog =
                new(
                    [
                        "Error",
                        "An error occurred when trying to create the user.",
                        "The operation will be cancelled."
                    ],
                    rightOption: "OK"
                );
            Window.AddElement(errorDialog);
            Window.ActivateElement(errorDialog);

            Window.RemoveElement(errorDialog);
            return false;
        }

        return true;
    }

    public static bool ViewCustomersTable()
    {
        try
        {
            IntSelector limitSelector =
                new("Select the number of customers to view", min: 3, max: 20, start: 10, step: 1);
            Window.AddElement(limitSelector);

            Window.ActivateElement(limitSelector);
            var limitResp = limitSelector.GetResponse();
            if (limitResp!.Status is not Status.Selected)
            {
                Window.RemoveElement(limitSelector);
                return false;
            }

            var customers = RepositoryImplementation.GetCustomers(limitResp.Value);

            TableView customersTable =
                new(
                    title: $"Customers (first {limitResp.Value})",
                    headers: new List<string>()
                    {
                        "email",
                        "first_name",
                        "last_name",
                        "password",
                        "orders_count",
                        "rank"
                    },
                    lines: customers
                );

            Window.AddElement(customersTable);
            Window.Render(customersTable);
            Window.Freeze();

            Window.RemoveElement(customersTable);
            return true;
        }
        catch (Exception)
        {
            Dialog errorDialog =
                new(
                    [
                        "Error",
                        "An error occurred when trying to fetch the customers.",
                        "The operation will be cancelled."
                    ],
                    rightOption: "OK"
                );
            Window.AddElement(errorDialog);
            Window.ActivateElement(errorDialog);

            Window.RemoveElement(errorDialog);
            return false;
        }
    }
}
