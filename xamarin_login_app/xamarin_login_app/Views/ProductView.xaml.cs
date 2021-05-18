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
	public partial class ProductView : ContentPage
	{
		ItemViewModel VM => (ItemViewModel)this.BindingContext;
		public ProductView(Item item)
		{
			if (item == null)
				throw new ArgumentNullException();

			InitializeComponent();

			VM.changeUi += updateComments;

			VM.updateItem(item).GetAwaiter();

			Title = item.name;

		}
		public void updateComments()
		{
			if (VM.Reviews.Count() == 0)
			{
				rlist.IsVisible = false;
				Overlay.IsVisible = true;
			}
			else
			{
				rlist.IsVisible = true;
				Overlay.IsVisible = false;
			}
		}

	}
}