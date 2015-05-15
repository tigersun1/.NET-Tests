using System;
using System.Threading;
using System.IO; 
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;



namespace comcash
{
    partial class TestData
	{
	 
		private int Replay;
		private int replayPoint;

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
			

		public void ErrorEmptyArgument()
		{
			Log.Error("Empty argument",false);
		}

		public void deleteListener(){
			File.Delete (ConfigTest.listenerPath);
		}

		public int getReplayPoint(){
			return replayPoint;
		}

		public void setReplayPoint (int val){
			replayPoint = val;
		}

		public int getReplay(){
			return Replay;
		}

		public void setReplay(int arg){
			Replay = arg;
		}
			

		public void EnterAmount(Window x, string amount){

			try{
				var field = x.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId (Variables.CalculatorTextBoxId));
				field.BulkText = "";
			    field.Enter (amount);
			    x.WaitWhileBusy ();
				Thread.Sleep(1000);
			 
			}

			catch (Exception e){		
				Log.Error(e.ToString(), true);
				return;
			}
			 
		}


		public string[] GetArgument (string str)
		{

			string [] split = str.Split(new Char [] {'('}); 
			if (split [1] == null)
				split [1] = "";

			else if (split [1][split [1].Length - 1] == ')')
				split[1] = split [1].Remove (split [1].Length - 1);

			for (int x = 0; x < split.Length; x++) {
				split [x] = split [x].ToLower (); 
				split [x] = split [x].Trim ();
			}

			return split;

		}


		public string[] GetTestCases()
		{
			if (!Directory.Exists(ConfigTest.TestCasesPath)) {
				messBox("ERROR: Incorrect test cases path");
				Environment.Exit(1);
			}

			string[] x = Directory.GetFiles (ConfigTest.TestCasesPath, "*.txt", SearchOption.AllDirectories);
			return x;

		}


		public string[] ParseTS(string Path)
		{
			string[] line = File.ReadAllLines (Path);
			return line;
		}
			
		}

}



