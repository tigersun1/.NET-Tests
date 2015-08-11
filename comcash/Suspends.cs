using System;
using System.Threading;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using System.Windows.Automation;
using System.Diagnostics;

namespace comcash
{
	public static class Suspends
	{

		//Suspends and partial suspends
		//comcash - application var
		public static TestStack.White.Application Suspend (TestStack.White.Application comcash)
		{
			var win = ConfigTest.getWindow(comcash);

			try{

				var suspendButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.SuspendButtonId));
				var totalLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(Variables.TotalLabelId));
				var total = ConfigTest.ConvertTotalLabel(totalLabel.Name);

				var balance = Payments.getBalanceDue(win);


				Thread.Sleep (500);
				suspendButton.Click ();
				Thread.Sleep(1000);

				if (total > balance){
					Payments.NoReceiptButtonClick(win);
				}

				ConfigTest.AcceptPayment (win);
				Fiddler.checkResponse("sale/suspend");

				return comcash;
			}

			catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}


		//recalls and voids suspend sales
		//comcash - application var, value - Void or empty value
		public static TestStack.White.Application recallSuspend (TestStack.White.Application comcash, string value)
		{
			var win = ConfigTest.getWindow(comcash);

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

					ConfigTest.AcceptPayment(win);

					Payments.ClickOnHomeButton(win);
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

