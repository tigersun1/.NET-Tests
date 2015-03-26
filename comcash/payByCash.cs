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
		public TestStack.White.Application payByCash(TestStack.White.Application comcash, string payType){

			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"),TestStack.White.Factory.InitializeOption.NoCache);

			try{
				var homeButt = win.Get<TestStack.White.UIItems.RadioButton> (SearchCriteria.ByAutomationId ("HomeNavButton"));
				homeButt.Click();
				Thread.Sleep(1000);

				if (!payType.Equals(""))
					EnterAmount(win,payType);

				var cashBut = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("TenderCashButton"));
				cashBut.Click();
				win.WaitWhileBusy();
				Thread.Sleep(2000);

				var dueLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("BalanceDueLabel"));
				double amount = double.Parse(dueLabel.Name);
				if (amount > 0)
					return comcash;

				var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("NoReceiptButton"));
				noReceiptButton.Click();
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

