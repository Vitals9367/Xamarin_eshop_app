using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using xamarin_login_app.Models;

namespace xamarin_login_app.Services
{
	class ImageService
	{

		public ImageService()
		{

		}

		public void getImage(Item item)
		{
			item.image =  ImageSource.FromUri(new Uri(Constants.RestIP + "/image?name=" + item.item_type.name + "&photo=" + item.image_file));
		}
	}
}
