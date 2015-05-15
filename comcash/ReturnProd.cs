using System;
using System.Threading;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace comcash
{
	partial class TestData
	{
		public TestStack.White.Application ReturnProd (TestStack.White.Application comcash, string args)
		{
			try{
				Window win = comcash.GetWindow(SearchCriteria.ByAutomationId(Variables.MainWindowId),TestStack.White.Factory.InitializeOption.NoCache);

				string prod, qty = null;

				if (args.Contains(",")){
					var str = args.Split(new Char [] {','});
					prod = str[0].Trim().ToLower();
					qty = str[1].Trim().ToLower();
				} else {
					prod = args;
				}

				Thread.Sleep(300);
				OpenProduct(comcash, prod);
				var saveButton = win.Get<TestStack.White.UIItems.Button> (SearchCriteria.ByAutomationId (Variables.SaveProductDetailsButtonId));
				if (!saveButton.IsOffScreen){
					if (qty != null)
						EnterAmount(win, qty);
					saveButton.Click();
				}
				
				return comcash;
			}catch (Exception e){
				Log.Error(e.ToString(), true);
				return comcash;
			}

		}
	}
}

