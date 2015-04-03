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
		public TestStack.White.Application Return (TestStack.White.Application comcash, string args)
		{
			Window win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"), TestStack.White.Factory.InitializeOption.NoCache);

			try{
				var adminButton = win.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByAutomationId("AdminNavButton"));
				Thread.Sleep(500);
				adminButton.Click();
				Thread.Sleep(1000);

				var journalButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("JournalButton"));
				Thread.Sleep(500);
				journalButton.Click();
				Thread.Sleep(1000);

				var ListBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId("JournalListBox"));

				var stopWatch = new Stopwatch();
				stopWatch.Start();
				while(stopWatch.ElapsedMilliseconds < 300000){
					//if (checkResponse())
					//	return comcash;
					if (ListBox.Enabled)
						break;
					if(stopWatch.ElapsedMilliseconds > 250000){
						Logger("<td><font color=\"red\">ERROR: Can't get list of transactions</font></td></tr>");
						SetFail(true);
						return comcash;
					}
				}

				var item = ListBox.AutomationElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, "ListBoxItem"));
				SelectionItemPattern sel = item.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
				sel.Select();

				if (args.Contains("void")){
					var voidButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText("Void"));
					Thread.Sleep(500);
					voidButton.Click();
					Thread.Sleep(1000);
					var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("NoReceiptButton"));
					noReceiptButton.Click();
					Thread.Sleep(1000);

					acceptReturn(comcash);

					return comcash;
				} else{
					var returnButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText("Return"));
					Thread.Sleep(500);
					returnButton.Click();
					Thread.Sleep(1000);

					var stopwatch = new Stopwatch();
					stopwatch.Start();
					while (stopwatch.ElapsedMilliseconds < 300000) {

					//	if (checkResponse())
					//		return comcash;
						var label = win.Get<TestStack.White.UIItems.Label> (SearchCriteria.ByAutomationId ("ErrorMessageLabel"));
						if (label.Name == "")
							break;
						if (stopWatch.ElapsedMilliseconds > 250000){
							Logger("<td><font color=\"red\">ERROR: Timeout</font></td></tr>");
							SetFail(true);
							return comcash;
						}

					}

					return comcash;
				}

			}

			catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}
	}
}

