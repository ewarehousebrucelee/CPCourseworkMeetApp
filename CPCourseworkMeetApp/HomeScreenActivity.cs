
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Square.Picasso;

namespace CPCourseworkMeetApp
{
	[Activity(Label = "HomeScreen", Theme="@style/CustomHomescreenActionBarTheme")]
	public class HomeScreen : Activity, ILocationListener
	{
		string _username;
		DrawerLayout _DrawerLayout;
		List<string> _leftItems = new List<string>();
		ArrayAdapter _leftAdapter;
		ListView _LeftDrawer;
		TextView txtCity, txtLastUpdate, txtDescription, txtHumidity, txtTime, txtCelcius;
		ImageView imgView;
		Android.Support.V4.App.ActionBarDrawerToggle _DrawerToggle;

		LocationManager locationManager;
		string provider;
		static double lat, lon;
		OpenWeatherMap openWeatherMap = new OpenWeatherMap();

		TextView _mainTextView;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			//RequestWindowFeature(Android.Views.WindowFeatures.RightIcon);
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.HomeScreen);

			locationManager = (LocationManager)GetSystemService(Context.LocationService);

			//Create criteria for the location service using the coarse constant
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Coarse,
				PowerRequirement = Power.Medium
			};

			//Set provider
			provider = locationManager.GetBestProvider(criteriaForLocationService, true);

			Location location = locationManager.GetLastKnownLocation(provider);
			if (location == null)
			{
				System.Diagnostics.Debug.WriteLine("No location");
			}

			_username = Intent.GetStringExtra("username");

			_DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.myDrawer);
			_LeftDrawer = FindViewById<ListView>(Resource.Id.leftListView);

			TextView txtGreeting = FindViewById<TextView>(Resource.Id.txtGreeting);
			txtGreeting.Text = "Welcome back, " + _username;

			_leftItems.Add("Contacts");
			_leftItems.Add("Midpoint Calculator");
			_leftItems.Add("Element 3");

			_DrawerToggle = new HomescreenActionBarDrawerToggle(this, _DrawerLayout, Resource.Drawable.ic_drawer, Resource.String.open_drawer, Resource.String.close_drawer);
			          
			_leftAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, _leftItems);
			_LeftDrawer.Adapter = _leftAdapter;

			_DrawerLayout.SetDrawerListener(_DrawerToggle);
			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetHomeButtonEnabled(true);

			_LeftDrawer.ItemClick += _LeftDrawer_ItemClick;
		}


		void _LeftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			switch (e.Id)
			{
				case 0:
					Toast.MakeText(this, "Contacts Clicked", ToastLength.Long).Show();
					var ContactsActivityIntent = new Intent(Application.Context, typeof(ContactsActivity));
					StartActivity(ContactsActivityIntent);
					break;
				case 1:
					Toast.MakeText(this, "Midpoint", ToastLength.Long).Show();
					var CoordinatesCalculatorIntent = new Intent(Application.Context, typeof(CoordinatesCalculatorActivity));
					StartActivity(CoordinatesCalculatorIntent);

					break;
				case 3:
					Toast.MakeText(this, "Element 3", ToastLength.Long).Show();
					break;
			}

			_DrawerLayout.CloseDrawers();
			_DrawerToggle.SyncState();
		}



		protected override void OnPostCreate(Bundle savedInstanceState)
		{
			base.OnPostCreate(savedInstanceState);
			_DrawerToggle.SyncState();
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
			_DrawerToggle.OnConfigurationChanged (newConfig);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (_DrawerToggle.OnOptionsItemSelected(item))
			{
				return true;
			}


			return base.OnOptionsItemSelected(item);
		}

		//Request location updates when the activity resumes
		protected override void OnResume()
		{
			base.OnResume();
			locationManager.RequestLocationUpdates(provider, 0, 5, this);
		}

		//Stop receiving updates when the activity is paused
		protected override void OnPause()
		{
			base.OnPause();
			locationManager.RemoveUpdates(this);
		}

		//When the location is changed, take the coordinates rounding to 4 sig figs
		public void OnLocationChanged(Location location)
		{
			lat = Math.Round(location.Latitude, 4);
			lon = Math.Round(location.Longitude, 4);

			new GetWeather(this, openWeatherMap).Execute(lat, lon);
		}

		public void OnProviderDisabled(string provider)
		{
			
		}

		public void OnProviderEnabled(string provider)
		{
			
		}

		public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
		{
			
		}

		//Get weather class, async task so it will run on background thread and result published
		//on UI thread
		private class GetWeather : AsyncTask<String, Java.Lang.Void, String>
		{
			private ProgressDialog pd = new ProgressDialog(Application.Context);
			private HomeScreen activity;
			OpenWeatherMap openWeatherMap;

			//Constructor
			public GetWeather(HomeScreen activity, OpenWeatherMap openWeatherMap)
			{
				this.activity = activity;
				this.openWeatherMap = openWeatherMap;
			}

			//Show "Please wait..." system alert 
			protected override void OnPreExecute()
			{
				base.OnPreExecute();
				pd.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
				pd.SetTitle("Please wait...");
				pd.Show();
			}

			//create url string, get http weather data from API and return
			protected override string RunInBackground(params string[] @params)
			{
				string stream = null;
				string urlString = Common.WeatherAPIRequest(lat, lon);

				Helper http = new Helper();
				stream = http.GetHttpData(urlString);

				return stream;
			}

			//Catch failure / dismiss progress dialog
			//Populate textviews with the correponding pieces of weather information
			//Populate imageview with image from api
			protected override void OnPostExecute(string result)
			{
				base.OnPostExecute(result);

				Console.WriteLine(result);



				if (result.Contains("Error: Not found city"))
				{
					pd.Dismiss();
					return;
				}

				//Deserialise
				openWeatherMap = JsonConvert.DeserializeObject<OpenWeatherMap>(result);

				pd.Dismiss();

				//Control
				activity.txtCity = activity.FindViewById<TextView>(Resource.Id.txtCity);
				activity.txtDescription = activity.FindViewById<TextView>(Resource.Id.txtDescription);
				activity.txtHumidity = activity.FindViewById<TextView>(Resource.Id.txtHumidity);
				activity.txtLastUpdate = activity.FindViewById<TextView>(Resource.Id.txtLastUpdate);
				activity.txtTime = activity.FindViewById<TextView>(Resource.Id.txtTime);
				activity.txtCelcius = activity.FindViewById<TextView>(Resource.Id.txtCelcius);
				activity.imgView = activity.FindViewById<ImageView>(Resource.Id.imageViewHomeScreen);

				//Add data
				activity.txtCity.Text = $"{openWeatherMap.name},{openWeatherMap.sys.country}";
				activity.txtLastUpdate.Text = $"Last Updated: {DateTime.Now.ToString("dd MMMM yyyy HH:mm")}";
				activity.txtDescription.Text = $"{openWeatherMap.weather[0].description}";
				activity.txtHumidity.Text = $"Humidity: {openWeatherMap.main.humidity} %";
				activity.txtTime.Text = $"Sunrise: {Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunrise)}/Sunset: {Common.UnixTimeStampToDateTime(openWeatherMap.sys.sunset)}";
				activity.txtCelcius.Text = $"{openWeatherMap.main.temp}°C";

				//Add image
				if (!String.IsNullOrEmpty(openWeatherMap.weather[0].icon))
				{
					Picasso.With(activity.ApplicationContext).Load(Common.GetImage(openWeatherMap.weather[0].icon))
						   .Into(activity.imgView);
				}
			}
		}
	}
}
