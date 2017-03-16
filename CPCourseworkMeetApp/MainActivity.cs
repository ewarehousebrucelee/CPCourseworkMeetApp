using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Java.Lang;
using Android.Views;
using Android.Content;

namespace CPCourseworkMeetApp
{
	[Activity(Label = "CPCourseworkMeetApp", MainLauncher = true)]
	public class MainActivity : Activity
{

		ProgressBar _progressBar;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			//Set title bar to invisible
			RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			//Link layout attributes
			var welcomeText = FindViewById<TextView>(Resource.Id.textViewWelcome);
			var pleaseText = FindViewById<TextView>(Resource.Id.textView2);
			var signUpButton = FindViewById<Button>(Resource.Id.buttonSignUp);
			var signInButton = FindViewById<Button>(Resource.Id.button1SignIn);
			_progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);


			//Create a typeface using custom font
			Typeface sand = Typeface.CreateFromAsset(Assets, "Quicksand-Regular.otf");

			//Set fonts
			welcomeText.SetTypeface(sand, TypefaceStyle.Normal);
			pleaseText.SetTypeface(sand, TypefaceStyle.Normal);
			signInButton.SetTypeface(sand, TypefaceStyle.Bold);
			signUpButton.SetTypeface(sand, TypefaceStyle.Bold);

			_progressBar.Visibility = ViewStates.Invisible;




			//When signin button is clicked
			signInButton.Click += (object sender, System.EventArgs e) =>
				{
					//Pull up dialog
					FragmentTransaction signintransaction = FragmentManager.BeginTransaction();
					dialog_signin signInDialog = new dialog_signin();
					signInDialog.Show(signintransaction, "dialog_fragment");
					_progressBar.Visibility = ViewStates.Visible;

				};
			//When signup button is clicked
			signUpButton.Click += (object sender, System.EventArgs e) =>
				{
					
					//Pull up dialog
					FragmentTransaction signuptransaction = FragmentManager.BeginTransaction();
					dialog_signup signUpDialog = new dialog_signup();
					signUpDialog.Show(signuptransaction, "dialog_fragment");
					_progressBar.Visibility = ViewStates.Visible;
					

				};

		}


	}
}