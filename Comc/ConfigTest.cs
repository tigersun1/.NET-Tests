using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using System.Collections.Generic;
using System.Windows.Forms;

namespace comcash
{
	public static class ConfigTest
	{
		static bool isKilled;
		static readonly public string TestCasesPath;
		static readonly public string folderPath;
		static readonly public string partPath;
		static readonly public string LogPath;
		static readonly public string serverName;
		static readonly public string appPath;
		static public bool connectStatus;
		static private int Replay;
		static private int replayPoint;

		static ConfigTest(){
			isKilled = false;
			connectStatus = true;
			Replay = 0;

			folderPath = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
			appPath = folderPath + @"\Comcash POS Application.exe";


			if (String.IsNullOrEmpty (System.Configuration.ConfigurationManager.AppSettings ["LogPath"])) {
				var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
				config.AppSettings.Settings ["LogPath"].Value = folderPath;
				config.Save(System.Configuration.ConfigurationSaveMode.Modified);
				System.Configuration.ConfigurationManager.RefreshSection ("appSettings");
			}

			partPath = Path.Combine (System.Configuration.ConfigurationManager.AppSettings ["LogPath"], "Testlog-" + DateTime.Now.ToString("d"));
			LogPath = Path.Combine(partPath, "Testlog.html");

			if (String.IsNullOrEmpty (System.Configuration.ConfigurationManager.AppSettings ["CasesPath"])) {
				messBox ("ERROR: No test cases path in the config file");
				Environment.Exit (1);
			}

			TestCasesPath = System.Configuration.ConfigurationManager.AppSettings ["CasesPath"];
			serverName = System.Configuration.ConfigurationManager.AppSettings ["ServerName"];

			if (String.IsNullOrEmpty (System.Configuration.ConfigurationManager.AppSettings ["ServerName"]) || !Inet.PingInternet()) {
				messBox ("ERROR: Server is not available or incorrect server name in config");
				Environment.Exit (1);
			}
				

//			if (!File.Exists (folderPath + @"\testconfig.txt")){
//				var write = new StreamWriter (folderPath + @"\testconfig.txt", true);
//				write.WriteLine ("Log path: " + folderPath);
//				write.WriteLine ("Cases path: ");
//				write.WriteLine ("Fiddler path: " + FiddlerPath);
//				write.WriteLine ("Server name: ");
//				write.Close();
//			} 
//
//			string[] lines = File.ReadAllLines (folderPath + @"\testconfig.txt");
//			foreach (string s in lines) {
//				if (s.ToLower ().StartsWith ("log")) {
//					string x = s.Substring (s.IndexOf (':') + 1);
//					x = x.Trim ();
//					if (String.IsNullOrEmpty (x))
//						x = folderPath;
//					partPath = Path.Combine (x, "Testlog-" + DateTime.Now.ToString("d"));
//					LogPath = Path.Combine(partPath, "Testlog.html");
//				} else if (s.ToLower ().StartsWith ("cases")) {
//					string x = s.Substring (s.IndexOf (':') + 1);
//					x = x.Trim ();
//					TestCasesPath = x;
//				} else if (s.ToLower ().StartsWith ("fiddler")) {
//					string x = s.Substring (s.LastIndexOf (':') + 1);
//					x = x.Trim ();
//					FiddlerPath = x;
//				} else if (s.ToLower ().StartsWith ("server")) {
//					string x = s.Substring (s.LastIndexOf (':') + 1);
//					x = x.Trim ();
//					serverName = x;
//				}
//			}
//
//			if (String.IsNullOrEmpty (TestCasesPath)) {
//				messBox ("ERROR: No test cases path in the config file");
//				Environment.Exit (1);
//			} else if (String.IsNullOrEmpty (FiddlerPath)) {
//				messBox ("ERROR: No fiddler path in the config file");
//				Environment.Exit (1);
//			} else if (String.IsNullOrEmpty (serverName) || !Inet.PingInternet()) {
//				messBox ("ERROR: Server is not available or incorrect server name in config");
//				Environment.Exit (1);
//			}  
				
		}

		static public void setKilled(bool n){
			isKilled = n;
		}

		static public bool getKilled(){
			return isKilled;
		}


		static public Window getWindow (TestStack.White.Application app){
			return app.GetWindow (SearchCriteria.ByAutomationId (Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);
		}



		 static public bool AcceptPayment(Window x){

			try{
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				while (stopwatch.ElapsedMilliseconds < 30000) {
					var label = x.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId (Variables.ErrorMessageLabelId));
					var c = x.Items.Exists(obj=>obj.Name.Contains("sure"));
					if (c){
						var WarWind = x.MdiChild(SearchCriteria.ByText(Variables.ReturnOptionsWindowText));
						var ApplyButt = WarWind.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ApplyReturnOptionsButtonId));
						ApplyButt.Click();
						continue;
					}
					else if (label.Name.StartsWith("Operation is"))
						continue;
					else if (label.Name.ToLower().Contains("change due")) {
						var okButt = x.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.CloseMessageButtonId));
						okButt.Click ();
						x.WaitWhileBusy();
						return true;
					} 
					else if (label.Name.ToLower().Contains("completed"))
						continue;
					else if (label.Name == "" || (ConvertTotalLabel(returnTotalLabel(x)) == 0))
						return true;
					else {
						Log.Error(label.Name, true);
						return false;
					}
				}
				return false;
			}
			catch (Exception e){
				Log.Error(e.ToString(), true);
				return false;
			}
		}


		public static string returnTotalLabel (Window win){
			return win.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId (Variables.TotalLabelId)).Name;
		}

		public static decimal ConvertTotalLabel (string arg){
			arg = arg.Remove (arg.IndexOf ("$"), 1);
			arg = arg.Remove (arg.IndexOf ("("), arg.Length - arg.IndexOf("("));
			decimal x;
			Decimal.TryParse (arg, out x);
			return x;
		}

		//launch application
		//returns application var
		public static TestStack.White.Application Launch()
		{
			TestStack.White.Application x = TestStack.White.Application.Launch (appPath);

			try{

				List<Window> list = x.GetWindows();
				var t = list.Exists(obj=>obj.Title.Contains("Instance issue"));
				if (t){
					Log.Error("POS is already running", false);
					return x;
				}


				Window window = x.GetWindow(SearchCriteria.ByAutomationId(Variables.PinWindowId), TestStack.White.Factory.InitializeOption.NoCache);
				if (window == null){
					Log.Error("POS is not opened", true);
					return x;
				}

				else{
					var b = window.Items.Exists(obj=>obj.Name.Contains("Config file not found"));
					if (b){
						Log.Error("No config file", true);
						return x;
					}

					return x;
				}
			}
			catch (Exception e){
				Log.Error(e.ToString(), true);
				return x;
			}
		}

		//logs that no argument
		public static void ErrorEmptyArgument()
		{
			Log.Error("Empty argument",false);
		}
			

		//getter of replayPoint
		public static int getReplayPoint(){
			return replayPoint;
		}

		//setter of replayPoint
		public static void setReplayPoint (int val){
			replayPoint = val;
		}

		public static int getReplay(){
			return Replay;
		}

		public static void setReplay(int arg){
			Replay = arg;
		}


		//gets argument from statement
		//str - statement
		public static string[] GetArgument (string str)
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


		//gets test cases from folder
		public static string[] GetTestCases()
		{
			if (!Directory.Exists(ConfigTest.TestCasesPath)) {
				messBox("ERROR: Incorrect test cases path");
				Environment.Exit(1);
			}

			string[] x = Directory.GetFiles (ConfigTest.TestCasesPath, "*.txt", SearchOption.AllDirectories);
			return x;

		}


		//parses test cases
		//path - file path
		public static string[] ParseTS(string Path)
		{
			string[] line = File.ReadAllLines (Path);
			return line;
		}



		public static bool messBox (string mess)
		{
			DialogResult result = MessageBox.Show (mess, "POS Testing", MessageBoxButtons.OKCancel);
			if (result == DialogResult.OK)
				return true;
			if (result == DialogResult.Cancel)
				return false;
			return false;
		}


	}
}

