using BusinessModels;
using BusinessServices.Repositories;
using Common;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.CustomerService
{
    /// <summary>
    /// This Service handles all the operations related to customer
    /// </summary>
    public class CustomerService : ICustomerRepository
    {
        private readonly IDataLayer _dataLayer;
        private readonly IEmailSenderRepository _emailSender;
        public CustomerService(IEmailSenderRepository emailSender, IDataLayer dataLayer)
        {
            _emailSender = emailSender;
            _dataLayer = dataLayer;
        }

        /// <inheritdoc/>
        public async Task<bool> CustomerMailSender(string vouchorCode)
        {
            try
            {
                int customerCount = _dataLayer.ListCustomers().Count;

                int batchSize = 2;
                int skip = 0;
                //using paggination if there is alot of data for better performance
                for (int i = 0; i < customerCount; i += batchSize)
                {
                    var customers = _dataLayer.ListCustomers()
                        .Skip(skip)
                        .Take(batchSize)
                        .Select(x => new Customer { Email = x.Email, CreatedDateTime = x.CreatedDateTime });

                    //Also we can send it in background Parallel using Thread
                    foreach (var customer in customers)
                    {
                        if (_dataLayer.ListOrders().Any(s => s.CustomerEmail == customer.Email))
                            continue;

                        await SendEmailToCustomer(customer, vouchorCode);
                    }
                  
                    skip += batchSize;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("CustomerMailSender Exception:{0} ,DateAndTime:{1}", ex.Message, DateTime.Now);
                return false;
            }

        }
        
       /// <inheritdoc/>
        public async Task<bool> SendEmailToNewCustomers()
        {
            try
            {
                int customerCount = _dataLayer.ListCustomers().Count(s => s.CreatedDateTime > DateTime.Now.AddDays(-1));

                int batchSize = 2;
                int skip = 0;

                //using paggination if there is alot of data for better performance
                for (int i = 0; i < customerCount; i += batchSize)
                {
                    // customers 
                    List<Customer> newCustomers = _dataLayer.ListCustomers()
                        .Where(s => s.CreatedDateTime > DateTime.Now.AddDays(-1))
                        .Skip(skip)
                        .Take(batchSize)
                        .Select(x => new Customer { Email = x.Email, CreatedDateTime = x.CreatedDateTime })
                        .ToList();
                   
                    //Also we can send it in background Parallel using Thread
                    newCustomers.ForEach(async customer => await SendNewCusomerEmail(customer));

                    skip += batchSize;
                }
                //All mails are sent! Success!
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendEmailToNewCustomers Exception:{0} ,DateAndTime:{1}", ex.Message, DateTime.Now);
                return false;
            }
        }


        /// <summary>
        /// Sends an email to a specific customer with a voucher code and a message.
        /// </summary>
        /// <param name="customer">The customer to whom the email will be sent.</param>
        /// <param name="vouchorCode">The voucher code to be included in the email.</param>
        /// <remarks>
        /// This method creates a new email message with the provided customer's email address as the recipient.
        /// If the application is in debug mode, the email will not be sent, and the email will be logged to the console.
        /// </remarks>
        private async Task SendEmailToCustomer(Customer customer, string vouchorCode)
        {
            //Create a new MailMessage
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();

            //Add customer to reciever list
            mailMessage.To.Add(customer.Email);
            //Add subject
            mailMessage.Subject = "We miss you as a customer";
            //Send mail from info@EO.com
            mailMessage.From = new System.Net.Mail.MailAddress(Email.MailFrom);
            //Add body to mail
            mailMessage.Body = "Hi " + customer.Email +
                     "<br>We miss you as a customer. Our shop is filled with nice products. Here is a voucher that gives you 50 kr to shop for." +
                     "<br>Voucher: " + vouchorCode +
                     "<br><br>Best Regards,<br>EO Team";
            #if DEBUG
            //Don't send mails in debug mode, just write the emails in console
            Console.WriteLine("Send customer mail to:" + customer.Email);
            #else
             await _emailSender.SendEmail(mailMessage);
            #endif
        }


        /// <summary>
        /// Sends a welcome email to a new customer.
        /// </summary>
        /// <param name="customer">The new customer to whom the email will be sent.</param>
        /// <remarks>
        /// This method creates a new email message with the provided customer's email as the recipient.
        /// The email contains a welcome message for the new customer.
        /// If the application is in debug mode, the email will not be sent, and the email will be logged to the console.
        /// </remarks>
        private async Task SendNewCusomerEmail(Customer customer)
        {
            System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage();
            //Add customer to reciever list
            mailMessage.To.Add(customer.Email);
            //Add subject
            mailMessage.Subject = "Welcome as a new customer at EO!";
            //Send mail from info@EO.com
            mailMessage.From = new System.Net.Mail.MailAddress(Email.MailFrom);
            //Add body to mail
            mailMessage.Body = "Hi " + customer.Email +
                     "<br>We would like to welcome you as customer on our site!<br><br>Best Regards,<br>EO Team";
            #if DEBUG
            //Don't send mails in debug mode, just write the emails in console
            Console.WriteLine("Send new customer mail to:" + customer.Email);
            #else
	           await _emailSender.SendEmail(mailMessage);
            #endif
        }
    }
}
