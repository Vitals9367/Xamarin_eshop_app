using System;
using System.Collections.Generic;
using System.Text;

namespace xamarin_login_app.Models
{
	public class Defined_Items
	{
		public int id { get; set; }
		public Item item { get; set; }
		public string size { get; set; }
		public int amount { get; set; }
		public int cart_item { get; set; }

		public Defined_Items()
		{

		}

	}
}
