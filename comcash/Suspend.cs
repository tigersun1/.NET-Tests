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
		public TestStack.White.Application Suspend (TestStack.White.Application comcash)
		{
			Window win = comcash.GetWindow (SearchCriteria.ByAutomationId ("Window"), TestStack.White.Factory.InitializeOption.NoCache);

			try{
				var suspendButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId ("SuspendButton"));
				var totalLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("TotalLabel"));
				var balanceLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("BalanceDueLabel"));

				var totalLabelstring = totalLabel.Name;
				var firstInd = totalLabelstring.IndexOf("$");
				totalLabelstring = totalLabelstring.Remove(firstInd,1);
				//messBox(totalLabelstring);
				var total = double.Parse(totalLabelstring);

				var balanceLbelstring = balanceLabel.Name;
				var secondInd = balanceLbelstring.IndexOf("$");
				balanceLbelstring = balanceLbelstring.Remove(secondInd,1);
				var balance = double.Parse(balanceLbelstring);

				Thread.Sleep (500);
				suspendButton.Click ();
				Thread.Sleep(1000);

				if (total > balance){
					var yesButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("ButtonYES")); 
					Thread.Sleep(500);
					yesButton.Click();
					Thread.Sleep(1000);
				}

				AcceptPayment (win);
				checkResponse("sale/suspend");

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

