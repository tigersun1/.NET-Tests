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
		public TestStack.White.Application payByAR (TestStack.White.Application comcash, string str)
		{
			try{
				Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"), TestStack.White.Factory.InitializeOption.NoCache);

				tenderOthers (win, str);

				var tenderOtherWin = win.MdiChild(SearchCriteria.ByText("TenderOtherWindow"));
				var button = tenderOtherWin.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText("Accounts Receivable"));
				button.Click();
				Thread.Sleep(600);

				var dueLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("BalanceDueLabel"));
				string due = dueLabel.Name;
				due = due.Remove(0,1);
				double amount = double.Parse(due);
				if (amount > 0)
					return comcash;

				var yesBut = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("ButtonYES"));
				yesBut.Click();
				Thread.Sleep(1000);

				AcceptPayment(win);

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

