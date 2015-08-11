using System;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application Continue (TestStack.White.Application comcash)
		{
			try{
				var win = comcash.GetWindow(SearchCriteria.ByAutomationId(Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);
				Payments.ClickOnHomeButton(win);
				var ContButt = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ContinueSaleButtonId));
				ContButt.Click();
				var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.NoReceiptButtonId));
				noReceiptButton.Click();

				ConfigTest.AcceptPayment(win);
				Fiddler.checkResponse("return");

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}
	}
}

