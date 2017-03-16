
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
using System.Data;

namespace CPCourseworkMeetApp
{
	[Activity(Label = "CoordinatesSetUpActivity")]
	public class CoordinatesSetUpActivity : Activity, ILocationListener
	{

		TextView _addresstext;
		Location _currentLocation;
		LocationManager _locationManager;

		string _locationProvider;
		TextView _locationText;

		Button _refreshButton;
		Button _submitCoordsButton;

		string _userName;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
			base.OnCreate(savedInstanceState);


			SetContentView(Resource.Layout.CoordinatesSetUpLayout);

			_userName = Intent.GetStringExtra("username");


			_addresstext = FindViewById<TextView>(Resource.Id.textViewDisplayUserAddress);
			_locationText = FindViewById<TextView>(Resource.Id.textViewDisplayUserCoordinates);

			_refreshButton = FindViewById<Button>(Resource.Id.buttonRefreshCoordinates);
			_submitCoordsButton = FindViewById<Button>(Resource.Id.buttonSubmitCoordinates);

			var introText = FindViewById<TextView>(Resource.Id.textViewThanksForSign);
			var titleText = FindViewById<TextView>(Resource.Id.textViewTitleConfirmLocation);

			introText.Text = "Thanks for signing up " + _userName + ". Please confirm your location and address below.";

			InitialiseLocationManager();

			Typeface sand = Typeface.CreateFromAsset(Assets, "Quicksand-Regular.otf");
			_addresstext.SetTypeface(sand, TypefaceStyle.Bold);
			_locationText.SetTypeface(sand, TypefaceStyle.Normal);
			introText.SetTypeface(sand, TypefaceStyle.Normal);
			titleText.SetTypeface(sand, TypefaceStyle.Normal);


			_refreshButton.Click += _refreshButton_Click;
			_submitCoordsButton.Click += _submitCoordsButton_Click;

		}

		void _refreshButton_Click(object sender, EventArgs e)
		{
			//refresh activity
			this.Recreate();
		}

		void _submitCoordsButton_Click(object sender, EventArgs e)
		{

			MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3307;database=cpcoursework; Uid=ewarehouse_bruce;Pwd=ms270199;charset=utf8");
			try
			{

				if (!con.Ping())
				{
					con.Open();
					Toast.MakeText(Application.Context, "Connection made", ToastLength.Short).Show();
					//Create command to insert new user
					MySqlCommand updateCoords = new MySqlCommand("UPDATE tblTest SET longitude=@long, latitude=@lat WHERE username=@username", con);

					updateCoords.Parameters.AddWithValue("@long", _currentLocation.Longitude);
					updateCoords.Parameters.AddWithValue("@lat", _currentLocation.Latitude);
					updateCoords.Parameters.AddWithValue("@username", _userName);
					updateCoords.ExecuteNonQuery();
					Toast.MakeText(this, "Coordinates updates succesfully", ToastLength.Short).Show();

					var HomeScreenNewIntent = new Intent(Application.Context, typeof(HomeScreen));
					HomeScreenNewIntent.PutExtra("username", _userName);
					StartActivity(HomeScreenNewIntent);
				}
			}
			catch (MySqlException ex)
			{
				Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
				Log.Debug("DEBUG", ex.ToString());
			}
			finally
			{
				con.Close();
			}
		}

		void InitialiseLocationManager()
		{
			_locationManager = (LocationManager)GetSystemService(LocationService);
			//Create criteria for the location service using the coarse constant
			Criteria criteriaForLocationService = new Criteria
			{
				Accuracy = Accuracy.Coarse,
				PowerRequirement = Power.Medium                   
			};

			_locationProvider = _locationManager.GetBestProvider(criteriaForLocationService, true);

		}

		async void OutputAddress()
		{
			if (_currentLocation == null)
			{
				_addresstext.Text = "Failed to get location";
			}

			Geocoder geocoder = new Geocoder(this);
			//Look up collection of Address objects for current location. max results of 10
			IList<Address> addressList = await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);
			//Select the first address if possible
			Address currentAddress = addressList.FirstOrDefault();

			if (currentAddress == null)
			{
				//RIP
				_addresstext.Text = "Failed to determine address";
			}
			else
			{
				//Build string to be printed to the screen
				StringBuilder deviceAddress = new StringBuilder();
				for (int i = 0; i < currentAddress.MaxAddressLineIndex; i++)
				{
					deviceAddress.AppendLine(currentAddress.GetAddressLine(i));
				}
				_addresstext.Text = deviceAddress.ToString();
			}
		}

		public override void OnBackPressed()
		{
			//Comment out "onbackpressed" so the user is forced to give coordinates
			///base.OnBackPressed();
		}

		protected override void OnResume()
		{
			base.OnResume();
			//Listen to the locationmanager when the activity comes into the foreground.
			if (_locationManager.IsProviderEnabled(_locationProvider))
			{
				//Tell system location service app wants to receive location updates
				_locationManager.RequestLocationUpdates(_locationProvider, 0, 5, this); //paramaters specify provider, time, distance thresholds.
			}
			else
			{
				Toast.MakeText(this, _locationProvider + " unavailible", ToastLength.Short);
			}

		}

		protected override void OnPause()
		{
			base.OnPause();
			//unsubscribe when activity goes into background.
			_locationManager.RemoveUpdates(this);
		}

		public void OnLocationChanged(Location location)
		{
			//When there is a change in location
			//Initialise that global var so we can use in getting the address
			_currentLocation = location;
			if (location == null)
			{
				_locationText.Text = "Failed to find location";
			}
			else
			{
				//output the coordinates
				_locationText.Text = string.Format(Convert.ToString(_currentLocation.Latitude) + "," + Convert.ToString(_currentLocation.Longitude));
				OutputAddress();
			}
		}

		public void OnProviderDisabled(string provider)
		{
			Toast.MakeText(this, "Provider disabled", ToastLength.Short);
		}

		public void OnProviderEnabled(string provider)
		{
			throw new NotImplementedException();
		}

		public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
		{
			Log.Debug("DEBUG", "Status changed");

		}


	}
}
