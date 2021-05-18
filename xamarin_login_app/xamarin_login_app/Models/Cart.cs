using System;
using System.Collections.Generic;
using System.Text;

namespace xamarin_login_app.Models
{
	class Cart
	{
		public int id { get; set; }
		public int user_id { get; set; }
		public List<Cart_Items> cartItems { get; set; }
		
		public Cart()
		{

		}

	}
}
