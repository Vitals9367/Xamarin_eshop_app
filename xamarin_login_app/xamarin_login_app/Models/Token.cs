using System;
using System.Collections.Generic;
using System.Text;

namespace xamarin_login_app.Models
{
	class Token
	{
		public string token { get; set; }

		public Token(string token)
		{
			this.token = token;
		}

	}
}
