using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using DataModel;
using System.Data.Entity;

namespace DataAccessLayer
{
    /// <summary>
    /// Sets the connection to the database
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// Gets the database connection string as defined in the caller's configuration file and 
        /// passes to the base class for all connections.
        /// For this solution, the WCFService web.config has an entry named "WPF-WCF-EF6"
        /// </summary>
        public DatabaseContext() : base(Helper.ConnectionString("WPF-WCF-EF6"))
        {

        }
    }

    /// <summary>
    /// Data Access class for CRUD operations.
    /// </summary>
    public class DataAccess
    {
        /// <summary>
        /// Gets all customers filtering last name by incoming characters
        /// </summary>
        /// <param name="containsLastName">String containing characters to filter last name on.  If null or empty, all customers are returned</param>
        /// <returns>List of Customer objects that meet the criteria</returns>
        public List<Customer> GetCustomersByLastName(string containsLastName)
        {
            using (var database = new DatabaseContext())
            {
                try
                {
                    if (string.IsNullOrEmpty(containsLastName) == false)
                    {
                        var customers = database.Customers
                         .Where(b => b.LastName.IndexOf(containsLastName) >= 0)
                         .OrderBy(b => b.LastName)
                            .ToList();
                        return customers;
                    }
                    else
                    {
                        var customers = database.Customers
                         .OrderBy(b => b.LastName)
                            .ToList();
                        return customers;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Gets all customers filtering last name by incoming characters
        /// </summary>
        /// <param name="containsLastName">String containing characters to filter last name on.  If null or empty, all customers are returned</param>
        /// <returns>List of Customer objects that meet the criteria</returns>
        public Customer GetLastCustomersByLastName()
        {
            using (var database = new DatabaseContext())
            {
                try
                {
                    var customer = database.Customers
                     .OrderBy(b => b.LastName)
                        .ToList()
                        .LastOrDefault();
                    return customer;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="newCustomer"></param>
        /// <returns>true=success, error message on failure</returns>
        public bool InsertCustomer(Customer newCustomer)
        {
            using (var database = new DatabaseContext())
            {
                try
                {
                    database.Customers.Add(newCustomer);
                    database.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }  
}
