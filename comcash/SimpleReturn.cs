using System;
using System.Threading;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application SimpleReturn (TestStack.White.Application comcash)
		{
			try{
				var win = comcash.GetWindow(SearchCriteria.ByAutomationId(Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);
				var adminButton = win.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByAutomationId(Variables.AdminNavButtonId));
				adminButton.Click();
				Thread.Sleep(1000);

				var ReturnButt = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.SimpleReturnButtonId));
				ReturnButt.Click();
				Thread.Sleep(300);

				return comcash;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}
	}
}

