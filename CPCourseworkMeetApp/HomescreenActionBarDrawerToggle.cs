using System;
using Android.App;
using Android.Support.V4.App;
using Android.Support.V4.Widget;

namespace CPCourseworkMeetApp
{
	public class HomescreenActionBarDrawerToggle : ActionBarDrawerToggle
	{
		Activity _Activity;
		public HomescreenActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, int imageResource, int openDrawerDesc, int closeDrawerDesc) 
			: base (activity, drawerLayout, imageResource, openDrawerDesc, closeDrawerDesc)
		{
			_Activity = activity;
		}

		public override void OnDrawerOpened(Android.Views.View drawerView)
		{
			base.OnDrawerOpened(drawerView);
			_Activity.ActionBar.Title = "Please Select From List";
		}

		public override void OnDrawerClosed(Android.Views.View drawerView)
		{
			base.OnDrawerClosed(drawerView);
			_Activity.ActionBar.Title = "";
		}

		public override void OnDrawerSlide(Android.Views.View drawerView, float slideOffset)
		{
			base.OnDrawerSlide(drawerView, slideOffset);
		}
	}
}
