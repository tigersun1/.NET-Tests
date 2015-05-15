using System;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using System.Threading;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application ChangeProdPrice (TestStack.White.Application comcash, string args)
		{
			try{
				var str = args.Split(new Char [] {','});
				var prod = str[0].Trim().ToLower();
				var val = str[1].Trim().ToLower();

				var win = OpenProduct (comcash, prod);
				var Button = win.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(Variables.ChangeProductPriceButtonId));
				Button.Click();
				Thread.Sleep(300);
				EnterAmount(win, val);
				SaveProdDetails(win);

				return comcash;

			} catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}

		}
	}
}
