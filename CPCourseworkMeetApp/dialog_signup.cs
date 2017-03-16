using System;
using System.IO;
using Android.App;
using Android.Widget;
using MySql.Data.MySqlClient;
using Android.Util;
using System.Data;
using Android.Graphics;
using Android.Views;
using Android.Content;

namespace CPCourseworkMeetApp
{


	public class dialog_signup : DialogFragment
	{
		private EditText _userNameText;
		private EditText _emailText;
		private EditText _passwordText;
		private Button _registerButton;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.DialogSignUp, container, false);

			_userNameText = view.FindViewById<EditText>(Resource.Id.editTextNewUser);
			_emailText = view.FindViewById<EditText>(Resource.Id.editTextNewEmail);
			_passwordText = view.FindViewById<EditText>(Resource.Id.editNewTextPassword);
			_registerButton = view.FindViewById<Button>(Resource.Id.buttonRegister);

			var textViewUserName = view.FindViewById<TextView>(Resource.Id.textViewNewUsername);
			var textViewEmail = view.FindViewById<TextView>(Resource.Id.textViewNewEmail);
			var textViewPassword = view.FindViewById<TextView>(Resource.Id.textViewNewPassword);

			Typeface sand = Typeface.CreateFromAsset(Application.Context.Assets, "Quicksand-Regular.otf");

			_registerButton.SetTypeface(sand, TypefaceStyle.Bold);
			_emailText.SetTypeface(sand, TypefaceStyle.Bold);
			_userNameText.SetTypeface(sand, TypefaceStyle.Bold);
			_passwordText.SetTypeface(sand, TypefaceStyle.Bold);
			textViewUserName.SetTypeface(sand, TypefaceStyle.Bold);
			textViewEmail.SetTypeface(sand, TypefaceStyle.Bold);
			textViewPassword.SetTypeface(sand, TypefaceStyle.Bold);




			_registerButton.Click += _registerButton_Click;

			return view;

		}

		void _registerButton_Click(object sender, EventArgs e)
		{
			//User has clicked register button
			MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3307;database=cpcoursework; Uid=ewarehouse_bruce;Pwd=ms270199;charset=utf8");

			try
			{

				if (!con.Ping())
				{
					con.Open();
					Toast.MakeText(Application.Context, "Connection made", ToastLength.Short).Show();
					//Create command to insert new user
					MySqlCommand newUser = new MySqlCommand("INSERT INTO tblTest(username,password) VALUES(@username,@password)", con);
					//Create command to check if the username already exists in table
					MySqlCommand checkExists = new MySqlCommand("SELECT COUNT(*) FROM tblTest WHERE (username = @username)", con);

					checkExists.Parameters.AddWithValue("@username", _userNameText.Text);
					//UserExist will be 0 if the username is not found.
					int UserExist = Convert.ToInt32(checkExists.ExecuteScalar());
					if (UserExist > 0)
					{
						//The username has been found
						Toast.MakeText(Application.Context, "Account could not be created: Username exists already.", ToastLength.Long).Show();
					}
					else
					{
						//Create account
						newUser.Parameters.AddWithValue("@username", _userNameText.Text);
						newUser.Parameters.AddWithValue("@password", _passwordText.Text);
						//Execute
						newUser.ExecuteNonQuery();

						Toast.MakeText(Application.Context, "Account successfully created!", ToastLength.Short).Show();
						//Launch activity to setup coords
						var newUserName = _userNameText.Text;
						var CoordinatesNewIntent = new Intent(Application.Context, typeof(CoordinatesSetUpActivity));
						CoordinatesNewIntent.PutExtra("username", newUserName);
						StartActivity(CoordinatesNewIntent);
					}

				}
				else
				{
					Console.WriteLine("A connection exists");
				}
			}
			catch (MySqlException ex)
			{
				//Debug
				Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Long).Show();
				Log.Debug("DEBUG", ex.ToString());
			}
			finally
			{
				//Close the connection
				con.Close();
			}
			//Close fragment
			this.Dismiss();

		}

		public override void OnActivityCreated(Android.OS.Bundle savedInstanceState)
		{
			Dialog.Window.RequestFeature(Android.Views.WindowFeatures.NoTitle);
			base.OnActivityCreated(savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_anim;

		}
	}
}
