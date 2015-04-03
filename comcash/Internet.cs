using System;
using System.Threading;


namespace comcash
{
	partial class TestData
	{
		public void Internet(string st)
		{

			if (st.StartsWith ("on") && !connectStatus) {
				Fiddler ("on");
				connectStatus = true;
			} else if (st.StartsWith ("off") && connectStatus) {
				Fiddler ("off");
				connectStatus = false;
			}
		}


		public bool PingInternet()
		{
			try{
				
				var ping = new System.Net.NetworkInformation.Ping();

				var result = ping.Send(serverName);
				if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
					return true;
				else
					return false;
			}

			catch (Exception e){
				Logger (e.Message);
				return false;
			}
		}
	}
}

