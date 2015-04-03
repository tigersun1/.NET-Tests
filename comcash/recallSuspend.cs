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
		public TestStack.White.Application recallSuspend (TestStack.White.Application comcash, string value)
		{
			Window win = comcash.GetWindow (SearchCriteria.ByAutomationId ("Window"), TestStack.White.Factory.InitializeOption.NoCache);

			try{

				var suspendedButton = win.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByAutomationId("SuspendedNavButton"));
				Thread.Sleep(500);
				suspendedButton.Click();
				Thread.Sleep(1000);

				var mySuspendedButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("SearchSuspendsButton"));
				var stopWatch = new Stopwatch();
				stopWatch.Start();
				while(stopWatch.ElapsedMilliseconds < 300000){
					if(mySuspendedButton.Enabled){
						checkResponse("sale");
						break;
					}
					if(stopWatch.ElapsedMilliseconds > 250000){
						checkResponse("sale");
						Logger("<td><font color=\"red\">ERROR: Can't get suspended list</font></td></tr>");
						SetFail(true);
						return comcash;
					}
				}

				var suspendedList = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId("SuspendsListBox"));
				var item = suspendedList.AutomationElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, "ListBoxItem"));
				SelectionItemPattern sel = item.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
				sel.Select();

				if (value.Contains("void")){
				
					var voidButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("VoidSuspendButton"));
					Thread.Sleep(500);
					voidButton.Click();
					Thread.Sleep(1000);

					var yes = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("ButtonYES"));
					Thread.Sleep(500);
					yes.Click();
					Thread.Sleep(1000);

					AcceptPayment(win);

					var homeButt = win.Get<TestStack.White.UIItems.RadioButton> (SearchCriteria.ByAutomationId ("HomeNavButton"));
					homeButt.Click();
					Thread.Sleep(1000);
				}
				else{
					var continueButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("ContinueSuspendButton"));
					Thread.Sleep(500);
					continueButton.Click();
					Thread.Sleep(1000);
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

