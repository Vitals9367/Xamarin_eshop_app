using System;
using System.Collections.Generic;
using System.Text;

namespace xamarin_login_app.Models
{
	public class Reviews
	{
		public int id { get; set; }
		public User user { get; set; }
		public int item { get; set; }
		public string comment { get; set; }
		public int rating { get; set; }
		public string date { get; set; }

		public Reviews()
		{

		}

	}
}
