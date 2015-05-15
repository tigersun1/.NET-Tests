using System;
using TestStack.White.UIItems.WPFUIItems;
using TestStack.White.UIItems.Finders;
using System.Threading;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application SetAllDiscount (TestStack.White.Application comcash, string arg)
		{

			try{

				var win = comcash.GetWindow(SearchCriteria.ByAutomationId(Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);
				ClickOnHomeButton(win);
				EnterAmount(win, arg);

				var DiscountButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.TotalDiscountButtonId));
				DiscountButton.Click();
				Thread.Sleep(300);

				var label = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(Variables.ErrorMessageLabelId));
				if(!label.IsOffScreen){
					var str = label.Text;
					Log.Error(str, true);
					return comcash;
				}

				return comcash;
				
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}
	}
}

