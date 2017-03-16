using System;
using System.Data;
using System.IO;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using MySql.Data.MySqlClient;
namespace CPCourseworkMeetApp
{
	public class dialog_signin : DialogFragment //dialog_signin inherits the DialogFragment class
	{
		EditText _usernametxt;
		EditText _txtpassword;
		Button _btnsign;
		ProgressBar _progressBar;

		public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.DialogSignIn, container, false); //Inflate the view using the DialogSignIn XML file, do not attach to root
			return view;
		}
		public override void OnActivityCreated(Android.OS.Bundle savedInstanceState)
		{

			Dialog.Window.RequestFeature(Android.Views.WindowFeatures.NoTitle); //Set title bar to invisible
			base.OnActivityCreated(savedInstanceState);
			Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_anim; //Set the animation
			_usernametxt = View.FindViewById<EditText>(Resource.Id.editTextUsername);
			_txtpassword = View.FindViewById<EditText>(Resource.Id.editTextPassword);
			_btnsign = View.FindViewById<Button>(Resource.Id.buttonsignindialog);
			_btnsign.Click += _btnsign_Click;
			_progressBar = View.FindViewById<ProgressBar>(Resource.Id.progressBar1);




		}

		void _btnsign_Click(object sender, EventArgs e)
		{
			MySqlConnection con = new MySqlConnection("Server=db4free.net;Port=3307;database=cpcoursework; Uid=ewarehouse_bruce;Pwd=ms270199;charset=utf8");

			try
			{
				if (con.State == ConnectionState.Closed)
				{
					con.Open();
					Toast.MakeText(Application.Context, "Connection made", ToastLength.Short).Show();

					MySqlCommand checkExists = new MySqlCommand("SELECT COUNT(*) FROM tblTest WHERE (username = @username)", con);
					MySqlCommand getPassword = new MySqlCommand("SELECT password FROM tblTest WHERE (username = @username)", con);

					getPassword.Parameters.AddWithValue("@username", _usernametxt.Text);
					MySqlDataReader passReader;

					checkExists.Parameters.AddWithValue("@username", _usernametxt.Text);
					int UserExist = Convert.ToInt32(checkExists.ExecuteScalar());
					//Check if the entered username is registered
					if (UserExist > 0)
					{
						//Check credentials
						passReader = getPassword.ExecuteReader();
						string realPassword = passReader.Read() ? passReader.GetString(0) : "Nothing Found";
						if (_txtpassword.Text == realPassword)
						{
							//Login Success
							Toast.MakeText(Application.Context, "Login Success", ToastLength.Short).Show();

							var HomeScreenNewIntent = new Intent(Application.Context, typeof(HomeScreen));
							HomeScreenNewIntent.PutExtra("username", _usernametxt.Text);
							StartActivity(HomeScreenNewIntent);


						}
						else
						{
							//Login Failed
							Toast.MakeText(Application.Context, "Invalid Password", ToastLength.Short).Show();
						}

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

		}

	}
}

