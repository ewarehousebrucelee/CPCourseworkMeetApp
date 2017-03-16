
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace CPCourseworkMeetApp
{
	public class dialog_viewContacts : DialogFragment
	{

		ListView _viewContactsListview;
		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.DialogViewContacts, container, false); //Inflate the view using the DialogSignIn XML file, do not attach to root

			_viewContactsListview = view.FindViewById<ListView>(Resource.Id.viewContactsListView);
			return view;
		}
		public override void OnActivityCreated(Android.OS.Bundle savedInstanceState)
		{

			Dialog.Window.RequestFeature(Android.Views.WindowFeatures.NoTitle); //Set title bar to invisible
			base.OnActivityCreated(savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_anim; //Set the animation

			var localContacts = Application.Context.GetSharedPreferences("MyContacts", FileCreationMode.Private);

			string usernameTag = "Username";
			string latitudeTag = "Latitude";
			string longitudeTag = "Longitude";
			string myContactTag = "myContact";

			List<Contact> contacts = new List<Contact>();

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

			//add list to list adapter
			ArrayAdapter adapter = new ArrayAdapter<Contact>(Application.Context, Android.Resource.Layout.SimpleListItem1, contacts);
			_viewContactsListview.Adapter = adapter;




		}

	}
}
