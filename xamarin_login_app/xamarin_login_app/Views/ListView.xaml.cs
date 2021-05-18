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
	public partial class ListView : ContentPage
	{

		ListViewModel VM => (ListViewModel)this.BindingContext;

		public ListView()
		{
			
			InitializeComponent();
			VM.changeUi += changeOverlay;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			Overlay.IsVisible = true;
			MainPage.IsVisible = false;
			await VM.GetProducts();
			Overlay.IsVisible = false;
			MainPage.IsVisible = true;

		}
		private void search1_TextChanged(object sender, TextChangedEventArgs e)
		{
			updateByName();
		}
		public void updateByName()
		{
			var list = VM.UpdateByName(search1.Text);

			if (list != null)
			{
				col1.ItemsSource = list;
			}
		}
		private void col1_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{

			//if (e.SelectedItem == null)
				//return;

			VM.ViewItem.Execute(null);
			col1.SelectedItem = null;
		}

		private void col1_Refreshing(object sender, EventArgs e)
		{
			Overlay.IsVisible = true;
			MainPage.IsVisible = false;
			VM.GetProducts().GetAwaiter();
			col1.EndRefresh();
		}

		public void changeOverlay()
		{
			if (VM.SearchList.Count == 0)
			{
				Overlay.IsVisible = false;
				MainPage.IsVisible = false;
				EmptyList.IsVisible = true;
			}
			else
			{
				Overlay.IsVisible = false;
				MainPage.IsVisible = true;
				EmptyList.IsVisible = false;
			}
		}
	}
}