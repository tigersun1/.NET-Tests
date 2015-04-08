using System;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using System.Threading;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application PLU (TestStack.White.Application comcash, string arg)
		{
			try{

				var win = comcash.GetWindow(SearchCriteria.ByAutomationId ("Window"), TestStack.White.Factory.InitializeOption.NoCache);
				ClickOnHomeButton(win);
				EnterAmount(win, arg);

				var pluButton = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId("PluButton"));
				pluButton.Click();
				Thread.Sleep(300);

				return comcash;

			}catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return comcash;
			}
		}
	}
}

