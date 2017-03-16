using System;
using System.Text;

namespace CPCourseworkMeetApp
{
	public class Common
	{
		//constants
		public static string WEATHER_API_KEY = "8af39cd44c1cfaa23a457ffe4c1181b2";
		public static string WEATHER_API_LINK = "http://api.openweathermap.org/data/2.5/weather";

		//Create link
		public static string WeatherAPIRequest(double lat, double lon)
		{
			StringBuilder sb = new StringBuilder(WEATHER_API_LINK);
			sb.AppendFormat("?lat={0}&lon={1}&APPID={2}&units=metric", lat, lon, WEATHER_API_KEY);
			return sb.ToString();
		}
		//Timestamp
		public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
		{
			DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			dt = dt.AddSeconds(unixTimeStamp).ToLocalTime();
			return dt;
		}
		//Get the image
		public static string GetImage(string icon)
		{
			return $"http://openweathermap.org/img/w/{icon}.png";
		}


	}
}
