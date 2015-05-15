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
				var homeButt = win.Get<TestStack.White.UIItems.RadioButton> (SearchCriteria.ByAutomationId (Variables.HomeNavButtonId));
				homeButt.Click();
				Thread.Sleep(300);

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return;
			}

		}
	}
}
