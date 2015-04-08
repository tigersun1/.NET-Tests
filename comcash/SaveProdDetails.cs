using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using System.Threading;

namespace comcash
{
	partial class TestData
	{
		public Window SaveProdDetails (Window win)
		{
			var saveButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId ("SaveProductDetailsButton"));
			saveButton.Click();
			Thread.Sleep(1000);
			return win;
		}
	}
}
