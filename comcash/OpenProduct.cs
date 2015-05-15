using System;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using System.Threading;

namespace comcash
{
	partial class TestData
	{
		public Window OpenProduct (TestStack.White.Application comcash, string ProdName)
		{
			Window win = comcash.GetWindow (SearchCriteria.ByAutomationId (Variables.MainWindowId), TestStack.White.Factory.InitializeOption.NoCache);
			try{
				var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId(Variables.BuyHomeListBoxId));
			var listItem = listBox.Items.Find(item=>item.Text.ToLower().StartsWith(ProdName));
			Thread.Sleep(1000);
			listItem.Click();
			win.WaitWhileBusy();
			Thread.Sleep(1000);

			return win;

			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return win;
			}
		}
	}
}
