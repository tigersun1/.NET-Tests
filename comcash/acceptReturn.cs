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
		public TestStack.White.Application acceptReturn (TestStack.White.Application comcash)
		{
			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"), TestStack.White.Factory.InitializeOption.NoCache);

			try{

				var continueButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("ContinueButton"));
				Thread.Sleep(500);
				continueButton.Click();
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

