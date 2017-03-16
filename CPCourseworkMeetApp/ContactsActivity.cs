
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;

namespace CPCourseworkMeetApp
{
	[Activity(Label = "ContactsActivity")]
	public class ContactsActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			//Set view
			SetContentView(Resource.Layout.Contacts);

			EditText _usernameBox = FindViewById<EditText>(Resource.Id.editTextEnterContact);
			EditText _latitudeBox = FindViewById<EditText>(Resource.Id.editTextContactLat);
			EditText _longitudeBox = FindViewById<EditText>(Resource.Id.editTextContactLon);

			Button autoCompleteButton = FindViewById<Button>(Resource.Id.buttonAutoEnterCoordinates);
			autoCompleteButton.Click += delegate {
				MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3307;database=cpcoursework; Uid=ewarehouse_bruce;Pwd=ms270199;charset=utf8");
				try
				{
					if (con.State == ConnectionState.Closed)
					{
						con.Open();
						Toast.MakeText(Application.Context, "Connection made", ToastLength.Short).Show();

						MySqlCommand checkExists = new MySqlCommand("SELECT COUNT(*) FROM tblTest WHERE (username = @username)", con);
						MySqlCommand getLatitude = new MySqlCommand("SELECT latitude FROM tblTest WHERE (username = @username)", con);
						MySqlCommand getlongitude = new MySqlCommand("SELECT longitude FROM tblTest WHERE (username = @username)", con);

						getLatitude.Parameters.AddWithValue("@username", _usernameBox.Text);
						getlongitude.Parameters.AddWithValue("@username", _usernameBox.Text);
						MySqlDataReader latReader;
						MySqlDataReader longReader;

						checkExists.Parameters.AddWithValue("@username", _usernameBox.Text);
						int UserExist = Convert.ToInt32(checkExists.ExecuteScalar());
						//Check if the entered username is registered
						if (UserExist > 0)
						{
							//Get coordinates
							latReader = getLatitude.ExecuteReader();
							string userLatitude = Convert.ToString(latReader.Read() ? latReader.GetString(0) : "0");
							con.Close();

							con.Open();
							longReader = getlongitude.ExecuteReader();

							string userLongitude = Convert.ToString(longReader.Read() ? longReader.GetString(0) : "0");

							//Change boxes to recieved coordinates
							_latitudeBox.Text = userLatitude;
							_longitudeBox.Text = userLongitude;

						}
						else
						{
							Toast.MakeText(Application.Context, "The username you have entered does not exist", ToastLength.Long).Show();
						}
					}
				}
				catch (Exception ex)
				{
					Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Long).Show();
					Log.Debug("DEBUG", ex.ToString());
				}
				finally
				{
					con.Close();

				}
			};

			Button submitButton = FindViewById<Button>(Resource.Id.buttonSubmitContact);
			submitButton.Click += delegate {

				string newContactUsername = _usernameBox.Text;

				string newContactLat = _latitudeBox.Text;

				string newContactLon = _longitudeBox.Text;



				//add new contact to shared preferences
				var localContacts = Application.Context.GetSharedPreferences("MyContacts", FileCreationMode.Private);
				var contactEdit = localContacts.Edit();

				string usernameTag = "Username";
				string latitudeTag = "Latitude";
				string longitudeTag = "Longitude";

				for (int i = 0; i < 20; i++)
				{
					usernameTag = usernameTag + i;
					latitudeTag = latitudeTag + i;
					longitudeTag = longitudeTag + i;

					string userName = localContacts.GetString(usernameTag, String.Empty);
					if (userName == String.Empty)
					{
						contactEdit.PutString(usernameTag, newContactUsername);
						contactEdit.PutString(latitudeTag, newContactLat);
						contactEdit.PutString(longitudeTag, newContactLon);
						contactEdit.Commit();
						//Success toast
						Toast.MakeText(this, "Contact added", ToastLength.Short).Show();
						break;

					}
					else
					{
						if (i == 19)
						{
							//fail toast
							Toast.MakeText(this, "Not enough space", ToastLength.Short).Show();
						}
					}
				}



				//Clear boxes
				_usernameBox.Text = "";
				_latitudeBox.Text = "";
				_longitudeBox.Text = "";

			};

			Button viewContactButton = FindViewById<Button>(Resource.Id.buttonViewContacts);
			viewContactButton.Click += delegate {
				//launch frag
				FragmentTransaction viewContactTransaction = FragmentManager.BeginTransaction();
				dialog_viewContacts contactsDialog = new dialog_viewContacts();
				contactsDialog.Show(viewContactTransaction, "dialog_fragment");
			};

		}
	}
}
