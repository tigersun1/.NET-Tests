using System;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using System.Threading;


namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application RemoveProd (TestStack.White.Application comcash, string prod)
		{
			try{

				var win = OpenProduct(comcash, prod);
				var Button = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId(Variables.RemoveProductButtonId));
				Button.Click();
				Thread.Sleep(300);
				return comcash;

			} catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}
		}
	}
}