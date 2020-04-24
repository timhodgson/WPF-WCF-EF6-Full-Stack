using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using DataModel;

namespace WPFFrontEnd
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AddCustomer()
        {
            InitializeComponent();

            #region Testing and Debugging
            // When debugging, load the first customer
            /*
                        if (Debugger.IsAttached)
                        {
                            LoadLastCustomer();
                        }
            */
            #endregion
        }


        #region Testing and Debugging
        /// <summary>
        /// *Test* WCF Service by getting a list of customers and pre-filling the form with the first customer.
        /// </summary>
        public void LoadLastCustomer()
        {
            try
            {
                string jsonString = string.Empty;
                string url = @"http://localhost:53472/Service/WCFService.svc/customers/GetAllCustomers";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    jsonString = reader.ReadToEnd();
                }

                Customer customer = JsonSerializer.Deserialize<List<Customer>>(jsonString).LastOrDefault();

                this.firstNameText.Text = customer.FirstName;
                this.lastNameText.Text = customer.LastName;
                this.phoneNumberText.Text = customer.PhoneNumber;
                this.emailAddressText.Text = customer.EmailAddress;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "An exception occurred");
            }
        }
        #endregion


        /// <summary>
        /// Add button was clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            #region Error Checking
            if (DataValidation() == false )
            {
                return;
            }
            #endregion

            #region Initialize
            string ResponseString = "";
            HttpWebResponse response = null;
            Customer newCustomer = new Customer
            {
                FirstName = this.firstNameText.Text,
                LastName = this.lastNameText.Text,
                PhoneNumber = this.phoneNumberText.Text,
                EmailAddress = this.emailAddressText.Text
            };
            #endregion

            #region Http POST call to endpoint to add new customer
            try
            {
                string url = @"http://localhost:53472/Service/WCFService.svc/customers/Add";

                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Accept = "application/json"; //"application/xml";
                request.Method = "POST";

                // serialize into JSON string
                var myContent = JsonSerializer.Serialize(newCustomer);

                var data = Encoding.ASCII.GetBytes(myContent);

                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                response = (HttpWebResponse)request.GetResponse();

                ResponseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    response = (HttpWebResponse)ex.Response;
                    ResponseString = "Some error occurred: " + response.StatusCode.ToString();
                }
                else
                {
                    ResponseString = "Some error occurred: " + ex.Status.ToString();
                }
            }
            #endregion

            #region Check response from endpoint
            if (ResponseString == "true")
            {
                Close();
            }
            else
            {
                MessageBox.Show(ResponseString, "Error");
            }
            #endregion
        }

        /// <summary>
        /// Performs data validation based on business rules.
        /// </summary>
        /// <returns></returns>
        private bool DataValidation()
        {
            if ( this.firstNameText.Text.Length < 1 )
            {
                MessageBox.Show("First name cannot be blank");
                this.firstNameText.Focus();
                return false;
            }
            if (this.lastNameText.Text.Length < 1)
            {
                MessageBox.Show("Last name cannot be blank");
                this.lastNameText.Focus();
                return false;
            }
            if (this.phoneNumberText.Text.Length < 12)
            {
                MessageBox.Show("Phone number must be at least 10 numbers");
                this.phoneNumberText.Focus();
                return false;
            }
            if (IsValidPhoneNumber(this.phoneNumberText.Text) == false)
            {
                MessageBox.Show("Phone number must be a valid format XXX-XXX-XXXX");
                this.phoneNumberText.Focus();
                return false;
            }
            if (this.emailAddressText.Text.Length < 1 )
            {
                MessageBox.Show("Email address cannot be blank");
                this.emailAddressText.Focus();
                return false;
            }
            if (IsValidEmailAddress(this.emailAddressText.Text) == false)
            {
                MessageBox.Show("Email address must match the pattern email@domain.com");
                this.emailAddressText.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Matches email address pattern
        /// </summary>
        /// <param name="emailAddress">String of an email address to validate.</param>
        /// <returns>true=is valid,false=not valid</returns>
        private bool IsValidEmailAddress(string emailAddress)
        {
            return CommonValidator(emailAddress,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return CommonValidator(phoneNumber, "^(\\d{3}\\-?\\d{3}\\-?\\d{4})$");
        }

        /// <summary>
        /// A common validation using Regex with timeout control.
        /// </summary>
        /// <param name="incomingString"></param>
        /// <param name="regexString"></param>
        /// <returns></returns>
        private bool CommonValidator(string incomingString, string regexString)
        {
            try
            {
                return Regex.IsMatch(incomingString,
                    regexString,
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Closes the dialog when the Cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
