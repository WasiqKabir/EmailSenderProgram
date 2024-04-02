using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Repositories
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Sends emails to customers who haven't placed orders yet, providing them with a voucher code.
        /// </summary>
        /// <param name="vouchorCode">The voucher code to be included in the email.</param>
        /// <returns>
        /// Returns a boolean value indicating whether the operation was successful.
        /// </returns>
        /// <remarks>
        /// This method retrieves a list of customers and checks if they have placed any orders. 
        /// If a customer hasn't placed any orders, an email is sent to them containing the provided voucher code.
        /// </remarks>
        Task<bool> CustomerMailSender(string vouchorCode);
        /// <summary>
        /// Sends emails to new customers who registered within the last day.
        /// </summary>
        /// <returns>
        /// Returns a boolean value indicating whether the operation was successful.
        /// </returns>
        /// <remarks>
        /// This method retrieves a list of new customers who registered within the last day and sends them a welcome email.
        /// If the application is in debug mode, the email will not be sent, and the email will be logged to the console.
        /// </remarks>
        Task<bool> SendEmailToNewCustomers();
    }
}
