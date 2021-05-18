using System;
using System.Collections.Generic;
using System.Text;

namespace xamarin_login_app.Models
{
	public class User
	{
		public int id { get; set; }
		public string public_id { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public string email { get; set; }

		public User(){}
		public User(string username, string password,string email)
		{
			this.public_id = public_id;
			this.username = username;
			this.password = password;
			this.email = email;
		}

	}
}
