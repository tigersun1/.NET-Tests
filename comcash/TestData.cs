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

namespace comcash
{
    partial class TestData
	{

		private bool Fail = false; 
		public int Errors = 0;
		private bool isKilled = false;

		private static string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		private string TestCasesPath = desktopPath + @"\TestSuits POS";
		string appPath = desktopPath + @"\1\Comcash POS Application.exe";

		public void setKilled(bool n){
			isKilled = n;
		}

		public bool getKilled(){
			return isKilled;
		}

		public void SetFail (bool x)
		{
			Fail = x;
			if (!Fail) {
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

