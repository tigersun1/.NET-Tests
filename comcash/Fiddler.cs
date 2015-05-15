using System;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using System.IO;
using System.Diagnostics;

namespace comcash
{
	public static class Fiddler
	{
		static readonly string FiddlerPath;
		static TestStack.White.Application app;

		static Fiddler(){
			FiddlerPath = ConfigTest.FiddlerPath + "\\Fiddler.exe";
		}

		static public void RunFiddler(){
			try{

				app = TestStack.White.Application.Launch (FiddlerPath);
				Window win = app.GetWindow(SearchCriteria.ByAutomationId(Variables.FiddlerWindow), TestStack.White.Factory.InitializeOption.NoCache);
				win.DisplayState = DisplayState.Minimized;
				app.WaitWhileBusy();

			} catch (Exception e){
				Log.Error(e.ToString(), true);
			}
		}

		static public void CloseFiddler(){
			try {
				
				app.Close();

			} catch (Exception e){
				Log.Error(e.ToString(), true);
			}
		}

		static public bool checkResponse (string arg){
			if (!Inet.PingInternet () || !ConfigTest.connectStatus)
				return true;

			Fiddler.FiddlerCommand ("\"request " + arg + "\"");

			if (!File.Exists (ConfigTest.listenerPath)) {
				Log.Error("No listener file in the folder",false);
				return false;
			}

			var res = File.ReadAllLines (ConfigTest.listenerPath);
			string str;
			if (res.Length > 0)
				str = res [res.Length - 1];
			else {
				Log.Error("Fiddler doesn't respond",false);
				return false;
			}


			str = str.ToLower ();
			str = str.Trim ();

			if (str == "true")
				return true;
			if (str == "false") {
				Log.SessionErrors = Log.SessionErrors + 1;
				Log.Error("Session error", false);
				Log.Link ("Error Response: ", "error(" + Log.SessionErrors + ").txt", ConfigTest.partPath + "\\error(" + Log.SessionErrors + ").txt");
				return false;
			} else
				return false;

		}


		static public void FiddlerCommand(string arg){
			if (!Directory.Exists(ConfigTest.FiddlerPath)) {
				//messBox("<td><font color=\"red\">ERROR: Incorrect fiddler path</font></td></tr>");
				Log.Error("Incorrect fiddler path", false);
				Environment.Exit(1);
			}

			string file = Path.Combine (ConfigTest.FiddlerPath, "ExecAction.exe");
			Process proc = Process.Start (file, arg);
			proc.WaitForExit ();
		}

	}
}

