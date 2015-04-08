using System.Threading;
using System;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public void ClickOnHomeButton (Window win)
		{
			try{
				var homeButt = win.Get<TestStack.White.UIItems.RadioButton> (SearchCriteria.ByAutomationId ("HomeNavButton"));
				homeButt.Click();
				Thread.Sleep(300);

			}catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return;
			}

		}
	}
}
