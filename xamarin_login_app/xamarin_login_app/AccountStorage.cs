using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Auth;

namespace xamarin_login_app
{
	public static class AccountStorage
	{
        public static void SaveCredentials(string userName, string password)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("Password", password);
                //AccountStore.Create().Save(account, App.AppName);
            }
        }

    }
}
