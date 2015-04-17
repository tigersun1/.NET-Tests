using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.IO; 
using TestStack;
using TestStack.White.Recording;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowStripControls;
using System.Collections.Generic;
using System.Net;


namespace comcash
{
    partial class TestData
	{

		private bool Fail; 
		public int Errors;
		public int SessionErrors;
		private bool isKilled;

		private string TestCasesPath;
		private string folderPath;
		static string partPath;
		private string LogPath;
		private string FiddlerPath = @"C:\Program Files (x86)\Fiddler2";
		private string listenerPath;
		private string serverName;
		private string listenerPathForFiddler;
		string appPath;
		private int Replay;
		private int replayPoint;
		private bool connectStatus;

		public TestData(){
			Fail = false;
			Errors = 0;
			SessionErrors = 0;
			isKilled = false;
			connectStatus = true;
			Replay = 0;

			folderPath = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
			listenerPath = folderPath + @"\listener.txt";
			if (listenerPath.Contains (" ")) {
				listenerPathForFiddler = "\"" + listenerPath + "\"";
			} else
				listenerPathForFiddler = listenerPath;
			appPath = folderPath + @"\Comcash POS Application.exe";

//test config settings

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
					if (String.IsNullOrEmpty (x)) {
						messBox ("ERROR: No test cases path in the config file");
						Environment.Exit (1);
					} else
						TestCasesPath = x;
				} else if (s.ToLower ().StartsWith ("fiddler")) {
					string x = s.Substring (s.LastIndexOf (':') + 1);
					x = x.Trim ();
					if (!String.IsNullOrEmpty (x))
						FiddlerPath = x;
				} else if (s.ToLower ().StartsWith ("server")) {
					string x = s.Substring (s.LastIndexOf (':') + 1);
					x = x.Trim ();
					if (String.IsNullOrEmpty (x)) {
						messBox ("ERROR: No server name in config file");
						Environment.Exit (1);
					} else
						serverName = x;
					if (!PingInternet()) {
						messBox ("ERROR: Server is not available or incorrect server name in config");
						Environment.Exit (1);
					}
				}
			} 
				
			Fiddler ("\"launch " + listenerPathForFiddler + " " + serverName + " " + partPath +"\"");
		}

//test config settings END

		public void ErrorEmptyArgument()
		{
			Logger ("<td><font color=\"red\">ERROR: Empty argument</font></td></tr>");
			SetFail (true);
		}

		public void deleteListener(){
			File.Delete (listenerPath);
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

		public void setKilled(bool n){
			isKilled = n;
		}

		public bool getKilled(){
			return isKilled;
		}

		public void SetFail (bool x)
		{
			Fail = x;
			if (Fail) {
				Errors++;
				Capture ();
			}
		}

		public bool GetFail()
		{
			return Fail;
		}
			

		public void EnterAmount(Window x, string amount){

			try{
				var field = x.Get<TestStack.White.UIItems.TextBox> (SearchCriteria.ByAutomationId ("CalculatorTextBox"));
				field.BulkText = "";
			    field.Enter (amount);
			    x.WaitWhileBusy ();
				Thread.Sleep(1000);
			 
			}

			catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
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
			if (!Directory.Exists(TestCasesPath)) {
				messBox("ERROR: Incorrect test cases path");
				System.Environment.Exit(1);
			}

			string[] x = Directory.GetFiles (TestCasesPath, "*.txt", SearchOption.AllDirectories);
			return x;

		}


		public string[] ParseTS(string Path)
		{
			string[] line = File.ReadAllLines (Path);
			return line;
		}
			
		}

}



