using System;
using System.Threading;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application Continue (TestStack.White.Application comcash)
		{
			try{
				var win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"), TestStack.White.Factory.InitializeOption.NoCache);
				ClickOnHomeButton(win);
				var ContButt = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("ContinueSaleButton"));
				ContButt.Click();
				var noReceiptButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("NoReceiptButton"));
				noReceiptButton.Click();

				AcceptPayment(win);
				checkResponse("return");

				return comcash;

			}catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}
	}
}

