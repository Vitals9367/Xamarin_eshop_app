using System;
using System.Collections.Generic;
using System.Text;

namespace xamarin_login_app.Models
{
	public class Cart_Items
	{
		public int id { get; set; }
		public int cart_id { get; set; }
		public Defined_Items defined_item { get; set; }
		public string date_added { get; set; }

		public Cart_Items()
		{

		}

	}
}
