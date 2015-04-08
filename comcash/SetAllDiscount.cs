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

				var win = comcash.GetWindow(SearchCriteria.ByAutomationId("Window"), TestStack.White.Factory.InitializeOption.NoCache);
				ClickOnHomeButton(win);
				EnterAmount(win, arg);

				var DiscountButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("TotalDiscountButton"));
				DiscountButton.Click();
				Thread.Sleep(300);

				var label = win.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId("ErrorMessageLabel"));
				if(!label.IsOffScreen){
					var str = label.Text;
					Logger ("<td><font color=\"red\">ERROR: " + str + "</font></td></tr>");
					SetFail (true);
					return comcash;
				}

				return comcash;
				
			}catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}
	}
}

