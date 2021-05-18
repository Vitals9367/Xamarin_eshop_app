using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using xamarin_login_app.Models;
using xamarin_login_app.Services;

namespace xamarin_login_app.ViewModels
{
	class ItemViewModel : BaseViewModel
	{

		public delegate void UpdateComments();
		public event UpdateComments changeUi;

		private string _type;
		public string type
		{
			set
			{
				this._type = value;
				OnPropertyChanged();
			}
			get
			{
				return this._type;
			}
		}

		private string _name;
		public string name
		{
			set
			{
				this._name = value;
				OnPropertyChanged();
			}
			get
			{
				return this._name;
			}
		}

		private string _description;
		public string description
		{
			set
			{
				this._description = value;
				OnPropertyChanged();
			}
			get
			{
				return this._description;
			}
		}

		private float _price;
		public float price
		{
			set
			{
				this._price = value;
				OnPropertyChanged();
			}
			get
			{
				return this._price;
			}
		}

		private bool _IsBusy;
		public bool IsBusy
		{
			set
			{
				this._IsBusy = value;
				OnPropertyChanged();
			}
			get
			{
				return this._IsBusy;
			}
		}

		private int _item_id;
		public int item_id
		{
			set
			{
				this._item_id = value;
				OnPropertyChanged();
			}
			get
			{
				return this._item_id;
			}
		}

		private string _selectedSize;
		public string selectedSize
		{
			set
			{
				this._selectedSize = value;
				OnPropertyChanged();
			}
			get
			{
				return this._selectedSize;
			}
		}

		private string _selectedRating;
		public string selectedRating
		{
			set
			{
				this._selectedRating = value;
				OnPropertyChanged();
			}
			get
			{
				return this._selectedRating;
			}
		}

		private string _selectedAmount;
		public string selectedAmount
		{
			set
			{
				this._selectedAmount = value;
				OnPropertyChanged();
			}
			get
			{
				return this._selectedAmount;
			}
		}

		private string _NewComment;
		public string NewComment
		{
			set
			{
				this._NewComment = value;
				OnPropertyChanged();
			}
			get
			{
				return this._NewComment;
			}
		}

		private ImageSource _image;
		public ImageSource image
		{
			set
			{
				this._image = value;
				OnPropertyChanged();
			}
			get
			{
				return this._image;
			}
		}

		private ObservableCollection<Reviews> _Reviews;
		public ObservableCollection<Reviews> Reviews
		{
			set
			{
				this._Reviews = value;
				OnPropertyChanged();
			}
			get
			{
				return this._Reviews;
			}
		}

		public Command BuyCommand { get; set; }
		public Command WriteCommentCommand { get; set; }

		public List<string> SizesList { get; set; }

		public List<string> AmountList { get; set; }

		public List<string> RatingList { get; set; }

		public ItemViewModel()
		{
			BuyCommand = new Command(async () => await BuyCommandAsync());
			WriteCommentCommand = new Command(async () => await WriteCommentAsync());
			NewComment = "";
			try
			{
				SizesList = new List<string>();
				SizesList.Add("S");
				SizesList.Add("M");
				SizesList.Add("L");
				SizesList.Add("XL");
				selectedSize = null;

				AmountList = new List<string>();
				AmountList.Add("1");
				AmountList.Add("2");
				AmountList.Add("3");
				AmountList.Add("4");
				selectedAmount = null;

				RatingList = new List<string>();
				RatingList.Add("1");
				RatingList.Add("2");
				RatingList.Add("3");
				RatingList.Add("4");
				RatingList.Add("5");
				selectedRating = null;

			}
			catch (Exception ex)
			{
				Application.Current.MainPage.DisplayAlert("Error", ex.Message , "OK");
			}
		}
		private async Task BuyCommandAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				if (selectedSize != null)
				{
					if(selectedAmount != null)
					{
						await new ProductService().addToCart(item_id,selectedSize,selectedAmount);
					}
					else
					{
						await Application.Current.MainPage.DisplayAlert("Error", "Please select amount you want!", "OK");
					}
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Please select size you want!", "OK");
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}

		private async Task WriteCommentAsync()
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				if (!NewComment.Trim().Equals(""))
				{
					if (selectedRating != null)
					{
						var result = await new ReviewsService().CreateReviews(item_id, int.Parse(selectedRating), NewComment);
						await GetComments();
						await Application.Current.MainPage.DisplayAlert("Sucess", "Comment!", "OK");
					}
					else
					{
						await Application.Current.MainPage.DisplayAlert("Error", "Please select rating!", "OK");
					}
				}
				else
				{
					await Application.Current.MainPage.DisplayAlert("Error", "Please write a comment!", "OK");
				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}

		public async Task updateItem(Item item)
		{
			item_id = item.id;
			name = item.name;
			description = item.description;
			price = item.price;
			image = item.image;
			await GetComments();
		}

		public async Task GetComments()
		{
			try
			{
				Reviews = new ObservableCollection<Reviews>();
				var items = await new ReviewsService().GetReviews(item_id);
				foreach (Reviews item in items)
					Reviews.Add(item);
				changeUi.Invoke();
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		}
	}
}
