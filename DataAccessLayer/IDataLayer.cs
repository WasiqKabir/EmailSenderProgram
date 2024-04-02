using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IDataLayer
    {
        /// <summary>
		/// Mockup method for all customers
		/// </summary>
        List<Customer> ListCustomers();
        /// <summary>
		/// Mockup method for listing all orders
		/// </summary>
        List<Order> ListOrders();
    }
}
