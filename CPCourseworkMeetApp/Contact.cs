using System;
namespace CPCourseworkMeetApp
{
	class Contact
	{
		public string UserName { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public Contact(string mName, double mLatitude, double mLongitude)
		{
			UserName = mName;
			Latitude = mLatitude;
			Longitude = mLongitude;
		}

		public override string ToString()
		{
			return UserName + " " + Latitude + " , " + Longitude;
		}
	}
}
