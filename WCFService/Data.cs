using System.Collections.Generic;
using DataModel;

namespace WCFService
{
    /// <summary>
    /// Test Data Class.  Initially used to wireframe the service and ensure it is working.
    /// </summary>
    public class Data
    {
        public static List<Customer> Customers = new List<Customer>
        {
            new Customer { FirstName = "Tim", LastName = "Hodgson", PhoneNumber = "267-820-8333", EmailAddress = "thodgson@gmail.com"},
        };
    }
}