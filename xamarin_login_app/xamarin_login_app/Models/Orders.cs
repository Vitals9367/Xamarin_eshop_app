using System;
using System.Collections.Generic;
using System.Text;

namespace xamarin_login_app.Models
{
	public class Orders
	{
		public int id { get; set; }
		public int user { get; set; }
		public List<Order_Items> order_items { get; set; }
		public string date { get; set; }
		public float price { get; set; }
		public bool paid { get; set; }

		public Orders()
		{

		}

	}
}
