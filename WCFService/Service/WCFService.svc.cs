using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using DataModel;
using DataAccessLayer;

namespace WCFService.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MainService
    {
        /// <summary>
        /// Gets all customer records 
        /// Example endpoint call: http://localhost:53472/Service/WCFService.svc/customers/GetAllCustomers
        /// </summary>
        /// <returns></returns>
        [OperationContract, WebGet(UriTemplate = "customers/GetAllCustomers", ResponseFormat = WebMessageFormat.Json)]
        public List<Customer> GetAllCustomers()
        {
            // Test internal data before going levels deeper and eventually to the db
            // return Data.Customers;
            DataAccess dataAccess = new DataAccess();
            return dataAccess.GetCustomersByLastName("");
        }

        /// <summary>
        /// Gets customer records based on specified parameters
        /// Example endpoint call: http://localhost:53472/Service/WCFService.svc/GetCustomersByLastName/Smith
        /// </summary>
        /// <param name="containsLastName">Last name contains specified string</param>
        /// <returns></returns>
        [OperationContract, WebGet(UriTemplate = "customers/GetCustomersByLastName/{containsLastName}", ResponseFormat = WebMessageFormat.Json)]
        public List<Customer> GetCustomersByLastName(string containsLastName)
        {
            // Test internal data before going levels deeper and eventually to the db
            // return Data.Customers;
            DataAccess dataAccess = new DataAccess();
            return dataAccess.GetCustomersByLastName(containsLastName);
        }

        /// <summary>
        /// Get the last customer sorted by last name
        /// </summary>
        /// <returns>A customer object that is the last customer in the list of all customers sorted by last name</returns>
        [OperationContract, WebGet(UriTemplate = "customers/GetLastCustomerByLastName", ResponseFormat = WebMessageFormat.Json)]
        public Customer GetLastCustomerByLastName()
        {
            DataAccess dataAccess = new DataAccess();
            return dataAccess.GetLastCustomersByLastName();
        }

        /// <summary>
        /// Adds a new customer record to the system
        /// </summary>
        /// <param name="newCustomer"></param>
        /// <returns></returns>
        [OperationContract, WebInvoke(Method = "POST", UriTemplate = "customers/Add", RequestFormat =WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public bool AddCustomer(Customer newCustomer)
        {
            DataAccess dataAccess = new DataAccess();
            return dataAccess.InsertCustomer(newCustomer);
        }
    }
}
