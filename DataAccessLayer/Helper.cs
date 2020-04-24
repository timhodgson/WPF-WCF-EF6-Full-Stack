using System.Configuration;

namespace DataAccessLayer
{
    /// <summary>
    /// Helper utility class.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Utility function for retrieving the connection string from a .config file
        /// </summary>
        /// <param name="name">Connection string name</param>
        /// <returns>Connection string</returns>
        public static string ConnectionString(string name)
        {
            ConnectionStringSettingsCollection collection = ConfigurationManager.ConnectionStrings;
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
