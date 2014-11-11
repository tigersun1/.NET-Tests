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
		public TestStack.White.Application addModifiers (TestStack.White.Application comcash, string arg)
		{
			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"), TestStack.White.Factory.InitializeOption.NoCache);

			try{
				string[] args = arg.Split (new Char[] {','}, StringSplitOptions.RemoveEmptyEntries);
				foreach (string s in args) {
					string str = s.Trim ();
					str = str.ToLower ();
					if (str == "save") {
						var saveButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId ("SaveProductDetailsButton"));
						if (saveButton.Enabled) {
							saveButton.Click ();
							Thread.Sleep (500);
							return comcash;
						} else {
							Logger ("<td><font color=\"red\">ERROR: Can't submit modifiers, SAVE button is disabled</font></td></tr>");
							SetFail (true);
							return comcash;
					}
				}
					else {
						var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId("ModifiersListBox"));
						var listItem = listBox.Items.Find(item=>item.Text.ToLower().StartsWith(str));
						Thread.Sleep(500);
						listItem.Click();
						Thread.Sleep(500);
					}
			}
			return comcash;
		}

			catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}
	}
}

