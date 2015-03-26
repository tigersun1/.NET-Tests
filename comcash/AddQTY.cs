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
		public TestStack.White.Application AddQTY (TestStack.White.Application comcash, string arg)
		{
			try{
			string[] args = arg.Split (new Char[] {','}, StringSplitOptions.RemoveEmptyEntries);

			string product = args[0];
			product = product.Trim ();
			product = product.ToLower ();
			string qty = args[1];
			qty = qty.Trim ();
			qty = qty.ToLower ();

			Window win = comcash.GetWindow (SearchCriteria.ByAutomationId ("Window"), TestStack.White.Factory.InitializeOption.NoCache);
			var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId("BuyHomeListBox"));
			var listItem = listBox.Items.Find(item=>item.Text.ToLower().StartsWith(product));
			Thread.Sleep(1000);
			listItem.Click();
			win.WaitWhileBusy();
			Thread.Sleep(1000);

			var clearButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByText ("C"));
			clearButton.Click ();
			Thread.Sleep (500);

			SendKeys.SendWait (qty);

			var saveButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId ("SaveProductDetailsButton"));
				saveButton.Click();
				Thread.Sleep(1000);

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

