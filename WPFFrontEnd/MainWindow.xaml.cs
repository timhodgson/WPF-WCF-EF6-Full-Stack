using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using DataModel;

namespace WPFFrontEnd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadCustomers();
        }

        /// <summary>
        /// Loads all customers via WCF Service
        /// </summary>
        public void LoadCustomers()
        {
            try
            {
                string jsonString = string.Empty;
                string url = @"http://localhost:53472/Service/WCFService.svc/customers/GetAllCustomers";

                Mouse.OverrideCursor = Cursors.Wait;
                this.listCustomers.Items.Clear();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    jsonString = reader.ReadToEnd();
                }

                List<Customer> customers = JsonSerializer.Deserialize<List<Customer>>(jsonString);

                if (customers.Count > 0)
                {
                    foreach (Customer customer in customers)
                    {
                        this.listCustomers.Items.Add(customer.FullInfo);
                    }
                }
                else
                {
                    this.listCustomers.Items.Add("No customers exist.  Please add a customer by clicking on the button below");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An exception occurred");
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        /// <summary>
        /// Add button was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer subWindow = new AddCustomer();
            subWindow.ShowDialog();
            LoadCustomers();
        }
    }
}
