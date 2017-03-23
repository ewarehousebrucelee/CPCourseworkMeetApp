
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Gms.Location.Places.UI;
using System.Device.Location;
using Newtonsoft.Json;
using Android.Gms.Maps.Model;

namespace CPCourseworkMeetApp
{
	[Activity(Label = "PickAPlaceActivity")]
	public class PickAPlaceActivity : Activity
	{

		private Button _pickAPlaceButton;
		private Button _shareViaWhatsAppButton;
		GeoCoordinate _midpoint;
		LatLng _southWestCorner = new LatLng(0, 0);
		LatLng _northEastCorner = new LatLng(0, 0);
		string _message;
		Android.Support.Design.Widget.TextInputEditText _placePickedMessage;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.PickAPlaceLayout);

			_midpoint = JsonConvert.DeserializeObject<GeoCoordinate>(Intent.GetStringExtra("coordinates"));
			_pickAPlaceButton = FindViewById<Button>(Resource.Id.pickAPlaceButton);
			_shareViaWhatsAppButton = FindViewById<Button>(Resource.Id.buttonPickAPlaceShareWithFriends);
			CreateLatLngBoundaries(_midpoint);
			_pickAPlaceButton.Click += _pickAPlaceButton_Click;
			_shareViaWhatsAppButton.Click += _shareViaWhatsAppButton_Click;
			_message = "Edit your message here!";

		}

		void CreateLatLngBoundaries(GeoCoordinate midpoint)
		{
			//1 degree of latitiude is around 111km
			//Therefore for 5km, 111 / 22.2 = 5km
			double latOffset = 1.0 / 22.2;
			double latMax = midpoint.Latitude + latOffset;
			double latMin = midpoint.Latitude - latOffset;

			//For longitude, 1 degree is 111km only at equator and it shrinks to zero at poles
			//So take account latitude using cos(lat).
			double lonOffset = latOffset * Math.Cos(midpoint.Latitude * Math.PI / 180);
			double lonMax = midpoint.Longitude + lonOffset;
			double lonMin = midpoint.Longitude - lonOffset;

			//Southwest corner = lonmin, latmin
			//Northeast corner = latmax, lonmax
			_southWestCorner.Latitude = latMin;
			_southWestCorner.Longitude = lonMin;
			_northEastCorner.Latitude = latMax;
			_northEastCorner.Longitude = lonMax;



		}

		//Launch place picker
		void _pickAPlaceButton_Click(object sender, EventArgs e)
		{
			var mapBuilder = new PlacePicker.IntentBuilder();
			LatLngBounds boundary = new LatLngBounds(_southWestCorner, _northEastCorner);
			mapBuilder.SetLatLngBounds(boundary);
			StartActivityForResult(mapBuilder.Build(this), 1);

		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (requestCode == 1 && resultCode == Result.Ok)
			{
				GetPlaceFromPicker(data);
			}
			base.OnActivityResult(requestCode, resultCode, data);
		}

		void GetPlaceFromPicker(Intent data)
		{
			TextView placePickedAddress = FindViewById<TextView>(Resource.Id.textViewPickAPlaceAddress);
			TextView placePickedName = FindViewById<TextView>(Resource.Id.textViewPickAPlaceName);
			_placePickedMessage = FindViewById<Android.Support.Design.Widget.TextInputEditText>(Resource.Id.textInputEditTextInvitationMessage);

			var placePicked = PlacePicker.GetPlace(this, data);

			if (placePicked != null)
			{
				placePickedName.Text = placePicked.NameFormatted.ToString();
				placePickedAddress.Text = placePicked.AddressFormatted.ToString();
				_message = "Hi, let's meet at"  + placePicked.NameFormatted + " , "
				                                             + placePicked.AddressFormatted.ToString() 
				                                             + ". Location calculated and message generated using Meet.";
			}

			_placePickedMessage.Text = _message;





		}

		void _shareViaWhatsAppButton_Click(object sender, EventArgs e)
		{
			Intent sendWhatsAppIntent = new Intent();
			sendWhatsAppIntent.SetPackage("com.whatsapp");
			sendWhatsAppIntent.SetAction(Intent.ActionSend);
			sendWhatsAppIntent.PutExtra(Intent.ExtraText, _placePickedMessage.Text);
			sendWhatsAppIntent.SetType("text/plain");
			StartActivity(sendWhatsAppIntent);

			_placePickedMessage.Text = "Edit your message here!";
		}
	}
}
