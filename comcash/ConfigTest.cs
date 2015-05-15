using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	public static class ConfigTest
	{
		static bool isKilled;
		static readonly public string TestCasesPath;
		static readonly public string folderPath;
		static readonly public string partPath;
		static readonly public string LogPath;
		static readonly public string FiddlerPath;
		static readonly public string listenerPath;
		static readonly public string serverName;
		static readonly public string listenerPathForFiddler;
		static readonly public string appPath;
		static public bool connectStatus;

		static ConfigTest(){
			isKilled = false;
			connectStatus = true;
			listenerPathForFiddler = @"C:\Program Files (x86)\Fiddler2";

			folderPath = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
			listenerPath = folderPath + @"\listener.txt";
			if (listenerPath.Contains (" ")) {
				listenerPathForFiddler = "\"" + listenerPath + "\"";
			} else
				listenerPathForFiddler = listenerPath;
			appPath = folderPath + @"\Comcash POS Application.exe";

			if (!File.Exists (folderPath + @"\testconfig.txt")){
				var write = new StreamWriter (folderPath + @"\testconfig.txt", true);
				write.WriteLine ("Log path: " + folderPath);
				write.WriteLine ("Cases path: ");
				write.WriteLine ("Fiddler path: " + FiddlerPath);
				write.WriteLine ("Server name: ");
				write.Close();
			} 

			string[] lines = File.ReadAllLines (folderPath + @"\testconfig.txt");
			foreach (string s in lines) {
				if (s.ToLower ().StartsWith ("log")) {
					string x = s.Substring (s.IndexOf (':') + 1);
					x = x.Trim ();
					if (String.IsNullOrEmpty (x))
						x = folderPath;
					partPath = Path.Combine (x, "Testlog-" + DateTime.Now.ToString("d"));
					LogPath = Path.Combine(partPath, "Testlog.html");
				} else if (s.ToLower ().StartsWith ("cases")) {
					string x = s.Substring (s.IndexOf (':') + 1);
					x = x.Trim ();
					TestCasesPath = x;
				} else if (s.ToLower ().StartsWith ("fiddler")) {
					string x = s.Substring (s.LastIndexOf (':') + 1);
					x = x.Trim ();
					FiddlerPath = x;
				} else if (s.ToLower ().StartsWith ("server")) {
					string x = s.Substring (s.LastIndexOf (':') + 1);
					x = x.Trim ();
					serverName = x;
				}
			} 
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
				while (stopwatch.ElapsedMilliseconds < 300000) {
					var label = x.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId (Variables.ErrorMessageLabelId));
					var c = x.Items.Exists(obj=>obj.Name.Contains("sure"));
					if (c){
						var WarWind = x.MdiChild(SearchCriteria.ByText(Variables.ReturnOptionsWindowText));
						var ApplyButt = WarWind.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ApplyReturnOptionsButtonId));
						ApplyButt.Click();
						continue;
					}
					else if (label.Name == "" | label.Name.StartsWith("Operation is"))
						continue;
					else if (label.Name.Contains ("Complete") || label.Name.ToLower().Contains("return") || label.Name.ToLower().Contains("completed")) {
						var okButt = x.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.CloseMessageButtonId));
						okButt.Click ();
						x.WaitWhileBusy();
						return true;
					} else {
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

	}
}

