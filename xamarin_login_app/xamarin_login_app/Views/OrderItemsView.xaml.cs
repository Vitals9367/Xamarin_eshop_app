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
	public partial class OrderItemsView : ContentPage
	{
		OrderItemsViewModel VM => (OrderItemsViewModel)this.BindingContext;
		public Orders ord;

		public OrderItemsView(Orders order)
		{
			if (order == null)
				throw new ArgumentNullException();

			InitializeComponent();
			VM.LoadingView += changeOverlay;
			ord = order;
			VM.updateItem(order);

			Title = "Order #" + order.id;

			if (order.paid)
			{
				StatusField.Text = "Payment Completed";
				StatusField.TextColor = Color.Green;
			}
			else
			{
				StatusField.Text = "Not Paid";
				StatusField.TextColor = Color.Red;
			}

			if(order.paid != false)
			{
				payment_button.IsEnabled = false;
				cancel_button.IsEnabled = false;
			}
		}
		public void changeOverlay(bool loading)
		{
			if (loading)
			{
				Overlay.IsVisible = true;
				MainPage.IsVisible = false;
			}
			else if (!loading)
			{
				Overlay.IsVisible = false;
				MainPage.IsVisible = true;
			}
		}

	private void col1_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{

			//if (e.SelectedItem == null)
			//return;

			VM.ViewItem.Execute(null);
			col1.SelectedItem = null;
		}
	}
}