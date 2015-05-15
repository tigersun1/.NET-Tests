using System;

namespace comcash
{
	public static class Inet
	{
		public static bool PingInternet()
		{
			try{

				var ping = new System.Net.NetworkInformation.Ping();

				var result = ping.Send(ConfigTest.serverName);
				if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
					return true;
				else
					return false;
			}

			catch (Exception e){
				Log.Error(e.ToString(), false);
				return false;
			}
		}

	public static void Internet(string st)
	{

			if (st.StartsWith ("on") && !ConfigTest.connectStatus) {
				Fiddler.FiddlerCommand (st);
				ConfigTest.connectStatus = true;
			} else if (st.StartsWith ("off") && ConfigTest.connectStatus) {
				Fiddler.FiddlerCommand (st);
				ConfigTest.connectStatus = false;
			} else 
				Log.Error ("Incorrect argumet " + st, false);
	}
}
}


