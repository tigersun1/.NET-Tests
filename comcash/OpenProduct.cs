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
			Window win = comcash.GetWindow (SearchCriteria.ByAutomationId ("Window"), TestStack.White.Factory.InitializeOption.NoCache);
			try{
			var listBox = win.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId("BuyHomeListBox"));
			var listItem = listBox.Items.Find(item=>item.Text.ToLower().StartsWith(ProdName));
			Thread.Sleep(1000);
			listItem.Click();
			win.WaitWhileBusy();
			Thread.Sleep(1000);

			return win;

			}catch (Exception e){
				Logger ("<td><font color=\"red\">ERROR: " + e + "</font></td></tr>");
				SetFail (true);
				return win;
			}
		}
	}
}
