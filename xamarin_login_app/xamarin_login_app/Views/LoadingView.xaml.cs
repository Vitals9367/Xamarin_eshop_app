using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace xamarin_login_app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoadingView : ContentPage
	{
		public LoadingView()
		{
			InitializeComponent();
		}
		protected override bool OnBackButtonPressed()
		{
			return true;
		}
	}
}