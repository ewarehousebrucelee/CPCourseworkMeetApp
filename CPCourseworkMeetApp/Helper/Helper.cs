using System;
using System.Text;
using Java.IO;
using Java.Net;

namespace CPCourseworkMeetApp
{
	//This is a helper object
	public class Helper
	{

		//Helper attributes
		static String stream = null;

		public String GetHttpData(String urlString) 


		//Check connection, create readers to get the data
		{
			try
			{
				URL url = new URL(urlString);
				using (var urlConnection = (HttpURLConnection)url.OpenConnection())
				{
					if (urlConnection.ResponseCode == HttpStatus.Ok)
					{
						BufferedReader r = new BufferedReader(new InputStreamReader(urlConnection.InputStream));
						StringBuilder sb = new StringBuilder();
						String line;
						while ((line = r.ReadLine()) != null)
						{
							sb.Append(line);
						}
						stream = sb.ToString();
						urlConnection.Disconnect();
					}
				}
			}
			catch (Exception ex)
			{
				System.Console.WriteLine(ex.Message);
			}
			return stream;
		}

	}
}
