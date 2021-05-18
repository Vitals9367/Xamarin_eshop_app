using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace xamarin_login_app.Models
{
	public class Item
	{
		public int id { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public string image_file { get; set; }
		public Item_Type item_type { get; set; }
		public float price { get; set; }

		public ImageSource image { get; set; }

		public Item() { }

	}
}
