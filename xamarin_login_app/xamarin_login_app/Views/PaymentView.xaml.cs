using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xamarin_login_app.Models;
using xamarin_login_app.ViewModels;

namespace xamarin_login_app.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaymentView : ContentPage
	{
		PaymentViewModel VM => (PaymentViewModel)this.BindingContext;
		public PaymentView(Orders order)
		{
			InitializeComponent();
			VM.LoadingView += changeOverlay;
			VM.CurrentOrder = order;
		}
		public void changeOverlay(bool loading)
		{
			if (loading) {
				Overlay.IsVisible = true;
				MainPage.IsVisible = false;
			}
			else if(!loading)
			{
				Overlay.IsVisible = false;
				MainPage.IsVisible = true;
			}
		}
	}
}