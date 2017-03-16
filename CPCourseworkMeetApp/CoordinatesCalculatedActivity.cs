
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Webkit;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.Device.Location;

namespace CPCourseworkMeetApp
{
	[Activity(Label = "CoordinatesCalculatedActivity")]
	public class CoordinatesCalculatedActivity : Activity
	{
		public GeoCoordinate coord1;
		public GeoCoordinate coord2;
		public GeoCoordinate coord3;
		public GeoCoordinate coord4;
		public GeoCoordinate coord5;

		public TextView midpointCoordinatesTextView;
		public TextView titleText;

		public IList<GeoCoordinate> geoCoordinates;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.CoordinatesCalculatedLayout);

			midpointCoordinatesTextView = FindViewById<TextView>(Resource.Id.CoordinatesCalculatedCoordinatesTextView);
			titleText = FindViewById<TextView>(Resource.Id.TheMidpointIs___TextView);


			RunGifAnimation();
			GetCoordinates();
			CalculateMidpoint();

		}

		public void RunGifAnimation()
		{
			var webLoadingIcon = FindViewById<WebView>(Resource.Id.webLoadingIcon);
			webLoadingIcon.LoadUrl("file:///android_asset/poi.gif");
			webLoadingIcon.SetBackgroundColor(new Android.Graphics.Color(0, 0, 0, 0));
			webLoadingIcon.Visibility = ViewStates.Visible;
			titleText.Visibility = ViewStates.Invisible;
			midpointCoordinatesTextView.Visibility = ViewStates.Invisible;

			Handler h = new Handler();
			Action closeGif = () =>
			{
				webLoadingIcon.Visibility = ViewStates.Gone;
				titleText.Visibility = ViewStates.Visible;
				midpointCoordinatesTextView.Visibility = ViewStates.Visible;

			};

			h.PostDelayed(closeGif, 5000);
		}

		public void CalculateMidpoint()
		{
			var midpoint = GeoMidPoint.GetCentralGeoCoordinate(geoCoordinates);
			midpointCoordinatesTextView.Text = Convert.ToString(midpoint);


		}

		public void GetCoordinates()
		{
			coord1 = JsonConvert.DeserializeObject<GeoCoordinate>(Intent.GetStringExtra("coord1"));
			coord2 = JsonConvert.DeserializeObject<GeoCoordinate>(Intent.GetStringExtra("coord2"));
			coord3 = JsonConvert.DeserializeObject<GeoCoordinate>(Intent.GetStringExtra("coord3"));
			coord4 = JsonConvert.DeserializeObject<GeoCoordinate>(Intent.GetStringExtra("coord4"));
			coord5 = JsonConvert.DeserializeObject<GeoCoordinate>(Intent.GetStringExtra("coord5"));

			//Add the coordinates to the geoCoordinate list
			geoCoordinates = new List<GeoCoordinate>();
			geoCoordinates.Add(coord1);
			geoCoordinates.Add(coord2);
			geoCoordinates.Add(coord3);
			geoCoordinates.Add(coord4);
			geoCoordinates.Add(coord5);

			Console.WriteLine(geoCoordinates);

		}



	}
}

