
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
using System.Device.Location;
using Newtonsoft.Json;

namespace CPCourseworkMeetApp
{
	[Activity(Label = "CoordinatesCalculatorActivity")]
	public class CoordinatesCalculatorActivity : Activity
	{
		Spinner spinnerFriends1;
		Spinner spinnerFriends2;
		Spinner spinnerFriends3;
		Spinner spinnerFriends4;
		Spinner spinnerFriends5;

		GeoCoordinate coord1;
		GeoCoordinate coord2;
		GeoCoordinate coord3;
		GeoCoordinate coord4;
		GeoCoordinate coord5;


		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.CoordinatesCalculatorLayout);

			var localContacts = Application.Context.GetSharedPreferences("MyContacts", FileCreationMode.Private);

			spinnerFriends1 = FindViewById<Spinner>(Resource.Id.spinnerFriends1);
			spinnerFriends2 = FindViewById<Spinner>(Resource.Id.spinnerFriends2);
			spinnerFriends3 = FindViewById<Spinner>(Resource.Id.spinnerFriends3);
			spinnerFriends4 = FindViewById<Spinner>(Resource.Id.spinnerFriends4);
			spinnerFriends5 = FindViewById<Spinner>(Resource.Id.spinnerFriends5);

			Button calculateCoordsButton = FindViewById<Button>(Resource.Id.buttonCalculateCoords);

			string usernameTag = "Username";
			string latitudeTag = "Latitude";
			string longitudeTag = "Longitude";
			string myContactTag = "myContact";

			List<Contact> contacts = new List<Contact>();

			//Create the list of contacts from the shared preference 
			for (int i = 0; i < 20; i++)
			{
				usernameTag = usernameTag + i;
				latitudeTag = latitudeTag + i;
				longitudeTag = longitudeTag + i;
				myContactTag = myContactTag + i;

				string username = localContacts.GetString(usernameTag, " ");
				string slatitude = localContacts.GetString(latitudeTag, " ");
				string slongitude = localContacts.GetString(longitudeTag, " ");

				double testvar;
				double latitude;
				double longitude;

				if (Double.TryParse(slatitude, out testvar))
				{
					latitude = testvar;
				}
				else
				{
					latitude = 0.0;
				}

				if (Double.TryParse(slongitude, out testvar))
				{
					longitude = testvar;
				}
				else
				{
					longitude = 0.0;
				}

				contacts.Add(new Contact(username, latitude, longitude));
			}
			//Create adapter based on the list of contacts and attach to all the spinners
			ArrayAdapter adapter = new ArrayAdapter<Contact>(Application.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, contacts);

			spinnerFriends1.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinnerFriends1_ItemSelected);
			spinnerFriends1.Adapter = adapter;
			spinnerFriends2.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerFriends2_ItemSelected);
			spinnerFriends2.Adapter = adapter;
			spinnerFriends3.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerFriends3_ItemSelected);
			spinnerFriends3.Adapter = adapter;
			spinnerFriends4.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerFriends4_ItemSelected);
			spinnerFriends4.Adapter = adapter;
			spinnerFriends5.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinnerFriends5_ItemSelected);
			spinnerFriends5.Adapter = adapter;

			calculateCoordsButton.Click += CalculateCoordsButton_Click;




			// Create your application here
		}

		//For each of the spinners get the selected item and create the geocoord out of it

		void spinnerFriends1_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			var contact = spinner.GetItemAtPosition(e.Position).Cast<Contact>();
			coord1 = new GeoCoordinate(contact.Latitude, contact.Longitude);

		}
		void spinnerFriends2_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			var contact = spinner.GetItemAtPosition(e.Position).Cast<Contact>();
			coord2 = new GeoCoordinate(contact.Latitude, contact.Longitude);
		}
		void spinnerFriends3_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			var contact = spinner.GetItemAtPosition(e.Position).Cast<Contact>();
			coord3 = new GeoCoordinate(contact.Latitude, contact.Longitude);
		}
		void spinnerFriends4_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			var contact = spinner.GetItemAtPosition(e.Position).Cast<Contact>();
			coord4 = new GeoCoordinate(contact.Latitude, contact.Longitude);
		}
		void spinnerFriends5_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;
			var contact = spinner.GetItemAtPosition(e.Position).Cast<Contact>();
			coord5 = new GeoCoordinate(contact.Latitude, contact.Longitude);
		}




		void CalculateCoordsButton_Click(object sender, EventArgs e)
		{


			var CoordinatesCalculatedIntent = new Intent(Application.Context, typeof(CoordinatesCalculatedActivity));
			CoordinatesCalculatedIntent.PutExtra("coord1", JsonConvert.SerializeObject(coord1));
			CoordinatesCalculatedIntent.PutExtra("coord2", JsonConvert.SerializeObject(coord2));
			CoordinatesCalculatedIntent.PutExtra("coord3", JsonConvert.SerializeObject(coord3));
			CoordinatesCalculatedIntent.PutExtra("coord4", JsonConvert.SerializeObject(coord4));
			CoordinatesCalculatedIntent.PutExtra("coord5", JsonConvert.SerializeObject(coord5));
			StartActivity(CoordinatesCalculatedIntent);
		}
	}
}
