using System;
using System.Collections.Generic;
using System.Text;

namespace xamarin_login_app.Models
{
	public class Order_Items
	{

		public int id { get; set; }
		public string date_added { get; set; }
		public int order_id { get; set; }
		public Defined_Items defined_item { get; set; }

		public Order_Items()
		{

		}

	}
}
