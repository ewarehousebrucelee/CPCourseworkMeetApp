
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
using Android.Locations;
using System.Threading.Tasks;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace CPCourseworkMeetApp
{
	[Activity(Label = "CoordinatesCalculatedActivity")]
	public class CoordinatesCalculatedActivity : Activity, IOnMapReadyCallback
	{
		public GeoCoordinate coord1;
		public GeoCoordinate coord2;
		public GeoCoordinate coord3;
		public GeoCoordinate coord4;
		public GeoCoordinate coord5;

		public TextView midpointCoordinatesTextView;
		public TextView titleText;

		Button nextButton;

		public GeoCoordinate geomidpoint;
		public Address addressMidpoint;

		public IList<GeoCoordinate> geoCoordinates;

		private GoogleMap gMap;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.CoordinatesCalculatedLayout);

			midpointCoordinatesTextView = FindViewById<TextView>(Resource.Id.CoordinatesCalculatedCoordinatesTextView);
			titleText = FindViewById<TextView>(Resource.Id.TheMidpointIs___TextView);

			RunGifAnimation();
			GetCoordinates();
			midpointCoordinatesTextView.Text = Convert.ToString(CalculateMidpoint());

			SetupMap();

			nextButton = FindViewById<Button>(Resource.Id.buttonGoToPlacePicker);

			nextButton.Click += NextButton_Click;

		}

		void NextButton_Click(object sender, EventArgs e)
		{
			//Start Placepicker activity, sending across the geomidpoint
			var PlacePickerNewIntent = new Intent(Application.Context, typeof(PickAPlaceActivity));
			PlacePickerNewIntent.PutExtra("coordinates", JsonConvert.SerializeObject(geomidpoint));
			StartActivity(PlacePickerNewIntent);
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

		public GeoCoordinate CalculateMidpoint()
		{
			//Calculate and return geomidpoint
			geomidpoint = GeoMidPoint.GetCentralGeoCoordinate(geoCoordinates);
			return geomidpoint;


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

		void SetupMap()
		{
			if (gMap == null)
			{
				FragmentManager.FindFragmentById<MapFragment>(Resource.Id.fragmentMap).GetMapAsync(this);
			}
		}

		public void OnMapReady(GoogleMap googleMap)
		{
			gMap = googleMap;
			LatLng nGeomidpoint = new LatLng(geomidpoint.Latitude, geomidpoint.Longitude);

			//Move and zoom camera to midpoint
			CameraUpdate midCamera = CameraUpdateFactory.NewLatLngZoom(nGeomidpoint, 10);
			gMap.MoveCamera(midCamera);

			//Create midpoint marker
			MarkerOptions midOptions = new MarkerOptions();
			midOptions.SetPosition(nGeomidpoint);
			midOptions.SetTitle("Your Geographical Midpoint");
			midOptions.SetSnippet("Get directions in the bottom right corner");
			gMap.AddMarker(midOptions);

		}
	}
}

