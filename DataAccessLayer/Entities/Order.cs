using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
	public class Order
	{
		public string CustomerEmail { get; set; }
		public DateTime OrderDatetime { get; set; }
	}
}
