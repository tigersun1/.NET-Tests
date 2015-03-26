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
		private bool isKilled;

		private static string desktopPath;
		private string TestCasesPath;
		private string folderPath;
		private string connectionName;
		static string partPath;
		private string LogPath;
		string appPath;
		private int Replay;
		private int replayPoint;

		public TestData(){
			Fail = false;
			Errors = 0;
			isKilled = false;
			Replay = 0;

			desktopPath = Environment.GetFolderPath (Environment.SpecialFolder.Desktop);
			folderPath = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
			appPath = folderPath + @"\Comcash POS Application.exe";

			if (File.Exists (folderPath + @"\testconfig.txt")) {
				string[] lines = File.ReadAllLines (folderPath + @"\testconfig.txt");
				foreach (string s in lines) {
					if (s.ToLower ().StartsWith ("connection")) {
						string x = s.Substring (s.LastIndexOf (':') + 1);
						x = x.Trim ();
						connectionName = x;
					} else if (s.ToLower ().StartsWith ("log")) {
						string x = s.Substring (s.IndexOf (':') + 1);
						x = x.Trim ();
						partPath = System.IO.Path.Combine(x, "TestLog-" + DateTime.Now.ToString ("D"));
					} else if (s.ToLower ().StartsWith ("cases")) {
						string x = s.Substring (s.IndexOf (':') + 1);
						x = x.Trim ();
						TestCasesPath = System.IO.Path.Combine( x.Substring(x.IndexOf('C'), x.LastIndexOf('\\')), x.Substring(x.LastIndexOf('\\') +1) );
					}
				}
			} 

			if (TestCasesPath == null)
				TestCasesPath = desktopPath + @"\TestSuits POS";
			if (partPath == null)	
				partPath = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Desktop), "TestLog-" + DateTime.Now.ToString ("D"));
			if (connectionName == null)	
				connectionName = "MD";

			LogPath = System.IO.Path.Combine(partPath, "testlogfile.html");
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


		public string GetArgument(string str)
		{
			string str2 = "";
			for (int x = 0; x < str.Length; x++) {
				if (str [x] == '(') {
					x++;
					if (str [x] == ')') {
						return str2;
					} else {
						do {
							str2 += str [x];
							x++;
						} while (str [x] != ')' && x < str.Length);
					}
					break;
				}
			}
			str2 = str2.Trim ();
			return str2;
		}


		public string[] GetTestCases()
		{
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



