using System;
using System.Threading;
using System.IO; 
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using System.Windows.Forms;



namespace comcash
{
    partial class TestData
	{
	 

		public TestData(){
			Replay = 0;
				
			if (String.IsNullOrEmpty (ConfigTest.TestCasesPath)) {
				messBox ("ERROR: No test cases path in the config file");
				Environment.Exit (1);
			} else if (String.IsNullOrEmpty (ConfigTest.FiddlerPath)) {
				messBox ("ERROR: No fiddler path in the config file");
				Environment.Exit (1);
			} else if (String.IsNullOrEmpty (ConfigTest.serverName) || !Inet.PingInternet()) {
				messBox ("ERROR: Server is not available or incorrect server name in config");
				Environment.Exit (1);
			}  

			Fiddler.FiddlerCommand ("\"launch " + ConfigTest.listenerPathForFiddler + " " + ConfigTest.serverName + " " + ConfigTest.partPath +"\"");
		}
			


	
	}
}



