using System;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application Suspend (TestStack.White.Application comcash)
		{
			Window win = comcash.GetWindow (SearchCriteria.ByAutomationId (Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);

			try{
				var suspendButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.SuspendButtonId));
				var totalLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(Variables.TotalLabelId));
				var balanceLabel = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(Variables.BalanceDueLabelId));


				var total = ConfigTest.ConvertTotalLabel(totalLabel.Name);

				var balanceLbelstring = balanceLabel.Name;
				var secondInd = balanceLbelstring.IndexOf("$");
				balanceLbelstring = balanceLbelstring.Remove(secondInd,1);
				decimal balance;
				Decimal.TryParse(balanceLbelstring, out balance);


				Thread.Sleep (500);
				suspendButton.Click ();
				Thread.Sleep(1000);

				if (total > balance){
					var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.NoReceiptButtonId));
					noReceiptButton.Click();
					Thread.Sleep(300);
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
	}
}

