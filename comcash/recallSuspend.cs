using System;
using System.Windows.Automation;
using System.Diagnostics;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application recallSuspend (TestStack.White.Application comcash, string value)
		{
			Window win = comcash.GetWindow (SearchCriteria.ByAutomationId (Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);

			try{

				var suspendedButton = win.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByAutomationId(Variables.SuspendedNavButtonId));
				Thread.Sleep(500);
				suspendedButton.Click();
				Thread.Sleep(1000);

				var mySuspendedButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.SearchSuspendsButtonId));
				var stopWatch = new Stopwatch();
				stopWatch.Start();
				while(stopWatch.ElapsedMilliseconds < 300000){
					if(mySuspendedButton.Enabled){
						Fiddler.checkResponse("sale");
						break;
					}
					if(stopWatch.ElapsedMilliseconds > 250000){
						Fiddler.checkResponse("sale");
						Log.Error("Can't get suspended list", true);
						return comcash;
					}
				}

				var suspendedList = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId(Variables.SuspendsListBoxId));
				var item = suspendedList.AutomationElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ClassNameProperty, "ListBoxItem"));
				SelectionItemPattern sel = item.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
				sel.Select();

				if (value.Contains("void")){
				
					var voidButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.VoidSuspendButtonId));
					Thread.Sleep(500);
					voidButton.Click();
					Thread.Sleep(1000);

					var yes = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ButtonYESId));
					Thread.Sleep(500);
					yes.Click();
					Thread.Sleep(1000);

					AcceptPayment(win);

					var homeButt = win.Get<TestStack.White.UIItems.RadioButton> (SearchCriteria.ByAutomationId (Variables.HomeNavButtonId));
					homeButt.Click();
					Thread.Sleep(1000);
				}
				else{
					var continueButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ContinueSuspendButtonId));
					Thread.Sleep(500);
					continueButton.Click();
					Thread.Sleep(1000);
				}

				return comcash;
			}

			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}

		}
	}
}

