using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_login_app.ViewModels;

namespace xamarin_login_app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomeTabsView : Shell
	{
		HomeTabsViewModel VM => (HomeTabsViewModel)this.BindingContext;
		public HomeTabsView()
		{
			InitializeComponent();
		}

		private void MenuItem_Clicked(object sender, EventArgs e)
		{
			VM.Logout.Execute(null);
		}
	}
}