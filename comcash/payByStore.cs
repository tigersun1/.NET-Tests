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
		public TestStack.White.Application payByStore (TestStack.White.Application comcash, string str)
		{
			try{
				Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"), TestStack.White.Factory.InitializeOption.NoCache);

				ClickOnHomeButton(win);
				tenderOthers (win, str);

				var tenderOtherWin = win.MdiChild(SearchCriteria.ByText("TenderOtherWindow"));
				var button = tenderOtherWin.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText("Store Credit"));
				button.Click();
				Thread.Sleep(500);

				var dueLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("BalanceDueLabel"));
				string due = dueLabel.Name;
				var index = due.IndexOf("$");
				due = due.Remove(index,1);
				double amount;
				Double.TryParse(due, out amount);
				if (amount > 0)
					return comcash;

				var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("NoReceiptButton"));
				noReceiptButton.Click();
				Thread.Sleep(1000);

				AcceptPayment(win);
				checkResponse("sale");

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

